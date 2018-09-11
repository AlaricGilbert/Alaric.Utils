using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alaric.Utils.JSON
{
    public class JsonNode : JsonElement
    {
        public readonly ArrayList Childs = new ArrayList();

        public bool IsArray = false;

        private readonly Hashtable _childTables = new Hashtable();

        public virtual void AddChild(JsonElement e)
        {
            if (_childTables.ContainsKey(e.Name))
                throw new Exception();
            Childs.Add(e);
            _childTables.Add(e.Name, e);
        }

        public virtual string[] GetChildNames()
        {
            string[] names = new string[_childTables.Count];
            for (int i = 0; i < _childTables.Count; i++)
                names[i] = ((KeyValuePair<string, JsonElement>) _childTables[i]).Key;
            return names;
        }

        public virtual JsonElement GetChildByName(string name) => (JsonElement) _childTables[name];

        public virtual void Remove(JsonElement e)
        {
            Childs.Remove(e);
            _childTables.Remove(e.Name);
        }

        public virtual void Remove(string name) => Remove(GetChildByName(name));

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            string prefix, suffix;
            if (IsArray)
            {
                prefix = IsArrayChild ? "[\r\n" : $"\"{Name}\":\r\n[\r\n";
                suffix = "\r\n]";
            }
            else
            {
                prefix = IsArrayChild ? "{\r\n" : $"\"{Name}\":\r\n" + "{\r\n";
                suffix = "\r\n}";
            }

            if (FatherElement == null)
                prefix = IsArray ? "[\r\n" : "{\r\n";
            foreach (JsonElement e in Childs)
            {
                if (e.GetType().ToString() == "Alaric.Utils.JSON.JsonElement")
                    b.AppendLine($"\t{e},");
                else
                    b.AppendLine("\t" + e.ToString().Replace("\n", "\n\t") + ",");
            }

            return prefix + b.ToString().TrimEnd().TrimEnd(',') + suffix;
        }
    }
}