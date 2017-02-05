using System;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using sharpcms.content.model;
using sharpcms.content.providers;
using sharpcms.database;

namespace sharpcms.content
{
    public class ContentFragmentService : IContentFragmentService
    {
        private readonly DbConnectionService _db;

        private readonly ContentFragmentProvider _fragmentProvider;

        public ContentFragmentService() : this(new DbConnectionService(), new ContentFragmentProvider())
        {
        }

        public ContentFragmentService(DbConnectionService db, ContentFragmentProvider fragmentProvider)
        {
            _db = db;

            _fragmentProvider = fragmentProvider;
        }

        public void CreateIfDoesNotExist(string name)
        {
            _db.CreateIfDoesNotExist(name);

            _fragmentProvider.CreateIfDoesNotExist(name);
        }

        public void DeleteIfDoesExist(string name)
        {
            _fragmentProvider.DeleteIfDoesExist(name);

            _db.DeleteIfDoesExist(name);
        }

        public void Insert(string name, ContentFragmentModel contentFragment)
        {
            using (var c = _db.GetConnection(name))
            {
                contentFragment.Created = DateTime.UtcNow;

                contentFragment.Updated = contentFragment.Created;

                c.Connection.Insert(contentFragment);
            }
        }

        public void Update(string name, ContentFragmentModel contentFragment)
        {
            using (var c = _db.GetConnection(name))
            {
                contentFragment.Updated = DateTime.UtcNow;

                c.Connection.Update(contentFragment);
            }
        }

        public void Delete(string name, ContentFragmentModel contentFragment)
        {
            contentFragment.Deleted = DateTime.UtcNow;

            Update(name, contentFragment);
        }

        public ContentFragmentModel GetById(string name, int id)
        {
            using (var c = _db.GetConnection(name))
            {
                return c.Connection.Query<ContentFragmentModel>($"SELECT * FROM [ContentFragment] WHERE [Id] = {id} AND [Deleted] IS NULL").FirstOrDefault();
            }
        }
    }
}
