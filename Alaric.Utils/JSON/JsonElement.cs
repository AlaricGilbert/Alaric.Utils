using System;

namespace Alaric.Utils.JSON
{
    public class JsonElement
    {
        public string Name { get; set; } = null;
        public string Value { get; set; } = null;
        public bool IsArrayChild { get; set; } = false;
        public JsonElement FatherElement { get; internal set; } = null;
        public override string ToString() => IsArrayChild ? $"\"{Value}\"" : $"\"{Name}\": \"{Value}\"";
    }
}