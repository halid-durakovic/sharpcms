using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace sharpcms.config
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigurationService(IConfigurationBuilder builder = null)
        {
            var builderIsNull = builder == null;

            builder = builder ?? new ConfigurationBuilder();

            if (builderIsNull)
                builder.AddJsonFile("settings.json");

            _configuration = builder.Build();
        }

        public string Get(string name)
        {
            return _configuration[name];
        }

        public T Get<T>(string alias = null) where T : new()
        {
            var typeName = alias ?? typeof(T).Name;

            var childOfRoot = _configuration.GetChildren().First(x => x.Key == typeName);

            var targetInstance = new T();

            RecursivelySetChildRootValues(targetInstance.GetType(), targetInstance, childOfRoot);

            return targetInstance;
        }

        private void SetTargetTypePropertyValue(Type targetType, object targetInstance, string propertyName, object propertyValue)
        {
            if (!HasTargetTypePropertyValue(targetType, targetInstance, propertyName,propertyValue))
            {
                var targetPropertyInfo = targetType.GetProperties().First(x => x.Name == propertyName);

                targetPropertyInfo.SetValue(targetInstance, propertyValue);
            }
        }

        private bool HasTargetTypePropertyValue(Type targetType, object targetInstance, string propertyName, object propertyValue)
        {
            if (targetType.GetProperties().Any(x => x.Name == propertyName))
            {
                var targetPropertyInfo = targetType.GetProperties().First(x => x.Name == propertyName);

                if (targetPropertyInfo.GetValue(targetInstance) != null)
                {
                    return true;
                }
            }

            return false;
        }

        private void RecursivelySetChildRootValues(Type targetType, object target, IConfigurationSection root)
        {
            foreach (var childOfRoot in root.GetChildren())
            {
                if (targetType.GetProperties().Any(x => x.Name == childOfRoot.Key))
                {
                    var propertyToBeSet = targetType.GetProperties().First(x => x.Name == childOfRoot.Key);

                    if (propertyToBeSet.PropertyType == typeof(string) && childOfRoot.Value is string)
                    {
                        propertyToBeSet.SetValue(target, childOfRoot.Value);
                    }
                    else if (propertyToBeSet.PropertyType.GetInterfaces().Any(x => x == typeof(IList)) && propertyToBeSet.PropertyType.IsConstructedGenericType)
                    {
                        var listGeneric = typeof(List<>);

                        var genericParameter = (propertyToBeSet.PropertyType.GetGenericArguments())[0];

                        var boundType = listGeneric.MakeGenericType(genericParameter);

                        var childOfTargetInstance = Activator.CreateInstance(boundType);

                        SetTargetTypePropertyValue(targetType, target, propertyToBeSet.Name, childOfTargetInstance);

                        if (childOfRoot.GetChildren().Any())
                        {
                            RecursivelySetChildRootValues(genericParameter, childOfTargetInstance, childOfRoot);
                        }
                    }
                    else
                    {
                        var childTypeOfTargetInstance = propertyToBeSet.PropertyType;

                        var childOfTargetInstance = Activator.CreateInstance(childTypeOfTargetInstance);

                        SetTargetTypePropertyValue(targetType, target, propertyToBeSet.Name, childOfTargetInstance);

                        if (childOfRoot.GetChildren().Any())
                        {
                            RecursivelySetChildRootValues(childTypeOfTargetInstance, childOfTargetInstance, childOfRoot);
                        }
                    }
                }
                else if (target.GetType().GetInterfaces().Any(x => x == typeof(IList)) && target.GetType().IsConstructedGenericType)
                {
                    var item = Activator.CreateInstance(targetType);

                    target.GetType().GetMethod("Add").Invoke(target, new[] { item });

                    RecursivelySetChildRootValues(targetType, item, childOfRoot);

                }
            }
        }
    }
}
