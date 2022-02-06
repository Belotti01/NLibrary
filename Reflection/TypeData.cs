using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace NL.Reflection {
    public static class TypeData {
        public static Dictionary<string, object> GetMembers<T>(this T obj, Type attributeType) {
            Dictionary<string, object> members = new();
            Type type = typeof(T);
            var allMembers = type.GetMembers()
                .Where(x => x.CustomAttributes
                    .Any(att => att.AttributeType == attributeType)
                );
            foreach (var member in allMembers) {
                members.Add(member.Name, (member as FieldInfo).GetValue(obj));
            }
            return members;
        }

        
    }
}
