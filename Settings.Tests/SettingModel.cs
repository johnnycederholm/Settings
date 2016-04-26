using System;

namespace Settings.Tests
{
    public class SettingModel
    {
        public string SomeString { get; set; }
        public int SomeInteger { get; set; }
        public bool SomeBoolean { get; set; }
        public char SomeChar { get; set; }
        public DateTime SomeDate { get; set; }
        public decimal SomeDecimal { get; set; }
        public double SomeDouble { get; set; }

        public ChildClass SomeChildObject { get; set; }
    }

    public class ChildClass
    {
        public string SomeChildString { get; set; }
    }
}
