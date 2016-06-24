using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.ExtensionMethods
{
    internal static class PropertyInfoEx
    {
        public static bool IsNavigationalProperty(this PropertyInfo p)
        {
            if (p.PropertyType.IsClass && p.PropertyType != typeof(string))
            {
                return true;
            }

            return false;
        }

        public static bool IsCollection(this PropertyInfo p)
        {
            var type = p.PropertyType;
            

            if (type.IsArray)
            {
                return true;
            }

            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();

                if(genericTypeDef == typeof(IEnumerable<>)
                    || genericTypeDef == typeof(ICollection<>)
                    || genericTypeDef == typeof(IList<>)
                    || genericTypeDef == typeof(List<>)
                    || genericTypeDef == typeof(Collection<>)
                    )
                {

                    return true;
                }
            }

            return false;
        }
    }
}
