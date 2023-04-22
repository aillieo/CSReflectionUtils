using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

        public static string GetDeclaration(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.GetTypeAliasOrName();
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(type.Name.Substring(0, type.Name.IndexOf('`')));

            Type[] arguments = type.GetGenericArguments();
            stringBuilder.Append("<");

            for (int i = 0; i < arguments.Length; i++)
            {
                if (i != 0)
                {
                    stringBuilder.Append(", ");
                }

                var t = arguments[i];

                if (t.IsGenericParameter)
                {
                    Type[] constraints = t.GetGenericParameterConstraints();

                    foreach (Type constraint in constraints)
                    {
                        GenericParameterAttributes gpa = constraint.GenericParameterAttributes;
                        GenericParameterAttributes variance = gpa & GenericParameterAttributes.VarianceMask;

                        if (variance != GenericParameterAttributes.None)
                        {
                            if ((variance & GenericParameterAttributes.Covariant) != 0)
                            {
                                stringBuilder.Append("in ");
                            }
                            else if ((variance & GenericParameterAttributes.Contravariant) != 0)
                            {
                                stringBuilder.Append("out ");
                            }
                        }
                    }
                }

                stringBuilder.Append(t.GetDeclaration());
            }

            stringBuilder.Append(">");

            return stringBuilder.ToString();
        }
    }
}
