using System.Collections.Generic;

namespace sharpcms.config.tests
{
    public class AnyTypeSetting
    {
        public string AnyTypeKey1 { get; set; } = string.Empty;

        public AnotherTypeSetting AnotherTypeSetting1 { get; set; } = null;

        public List<AnotherTypeSetting> AnotherTypeSettingList1 { get; set; } = null;
    }
}