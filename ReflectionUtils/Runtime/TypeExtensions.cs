using System;
using System.Collections.Generic;

namespace AillieoUtils.CSReflectionUtils
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> typeToAlias = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(object), "object" },
            { typeof(string), "string" },
            { typeof(void), "void" },
        };

        public static string GetTypeAliasOrName(this Type type)
        {
            if (type.BaseType == typeof(Array))
            {
                return $"{GetTypeAliasOrName(type.GetElementType())}[]";
            }

            if (typeToAlias.TryGetValue(type, out string name))
            {
                return name;
            }

            return type.Name;
        }
    }
}
