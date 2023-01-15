using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AillieoUtils.CSReflectionUtils
{
    public static class ReflectionUtils
    {
        public const BindingFlags flagAllAccessible = BindingFlags.Instance | BindingFlags.Static |
                                                            BindingFlags.NonPublic | BindingFlags.Public;

        public static IEnumerable<Type> GetAllTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes());
        }

        public static IEnumerable<Type> GetInheritanceChain(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }

        public static IEnumerable<FieldInfo> GetAllAccessibleFields(Type type)
        {
            return type.GetFields(flagAllAccessible);
        }

        public static IEnumerable<PropertyInfo> GetAllAccessibleProperties(Type type)
        {
            return type.GetProperties(flagAllAccessible);
        }

        public static IEnumerable<MethodInfo> GetAllAccessibleMethods(Type type)
        {
            return type.GetMethods(flagAllAccessible);
        }

        public static IEnumerable<FieldInfo> GetAllAccessibleFields(Type type, Predicate<FieldInfo> filter)
        {
            return GetAllAccessibleFields(type).Where(f => filter(f));
        }

        public static IEnumerable<PropertyInfo> GetAllAccessibleProperties(Type type, Predicate<PropertyInfo> filter)
        {
            return GetAllAccessibleProperties(type).Where(p => filter(p));
        }

        public static IEnumerable<MethodInfo> GetAllAccessibleMethods(Type type, Predicate<MethodInfo> filter)
        {
            return GetAllAccessibleMethods(type).Where(m => filter(m));
        }

        public static FieldInfo GetFieldEx(Type type, string name)
        {
            return GetAllAccessibleFields(type, f => string.Equals(f.Name, name, StringComparison.Ordinal)).FirstOrDefault();
        }

        public static PropertyInfo GetPropertyEx(Type type, string name)
        {
            return GetAllAccessibleProperties(type, p => string.Equals(p.Name, name, StringComparison.Ordinal)).FirstOrDefault();
        }

        public static MethodInfo GetMethodEx(Type type, string name)
        {
            return GetAllAccessibleMethods(type, m => string.Equals(m.Name, name, StringComparison.Ordinal)).FirstOrDefault();
        }

        public static IEnumerable<Type> FindSubTypes(Type baseType, bool includeAbstract = true)
        {
            var allTypes = GetAllTypes();
            if (!includeAbstract)
            {
                allTypes = allTypes.Where(t => !t.IsAbstract);
            }

            if (baseType.IsGenericType)
            {
                return allTypes
                    .Where(t => t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == baseType);
            }
            else
            {
                return allTypes
                    .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract);
            }
        }

        public static IEnumerable<Type> FindImplementations(Type interfaceType)
        {
            if (interfaceType.IsGenericType)
            {
                return GetAllTypes()
                    .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == interfaceType));
            }
            else
            {
                return GetAllTypes()
                    .Where(t => t.GetInterfaces().Contains(interfaceType));
            }
        }

        public static IEnumerable<KeyValuePair<T, Type>> GetAllTypesWithAttribute<T>(bool inherit = true)
            where T : Attribute
        {
            return GetAllTypes()
                .SelectMany(
                t => t.GetCustomAttributes<T>(inherit),
                (type, attr) => new KeyValuePair<T, Type>(attr, type));
        }

        private static readonly char[] pathSeparators = new char[] { '.', '[', ']' };

        public static bool GetFieldOrPropertyValueByPath(object source, string path, out object value)
        {
            string[] pathSegments = path.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < pathSegments.Length; i++)
            {
                if (source == null)
                {
                    value = default;
                    return false;
                }

                string pathSegment = pathSegments[i];

                GetFieldOrPropertyValue(source, pathSegment, out source);
            }

            value = source;
            return true;
        }

        public static bool SetFieldOrPropertyValueByPath(object source, string path, object value)
        {
            string[] pathSegments = path.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < pathSegments.Length - 1; i++)
            {
                if (source == null)
                {
                    return false;
                }

                string pathSegment = pathSegments[i];

                GetFieldOrPropertyValue(source, pathSegment, out source);
            }

            string lastSegment = pathSegments[pathSegments.Length - 1];
            SetFieldOrPropertyValue(source, lastSegment, value);

            return true;
        }

        private static bool GetFieldOrPropertyValue(object source, string pathSegment, out object value)
        {
            Type type = source.GetType();
            if (type.IsArray && int.TryParse(pathSegment, NumberStyles.Integer, CultureInfo.InvariantCulture, out int index))
            {
                Array array = source as Array;
                value = array.GetValue(index);
                return true;
            }
            else
            {
                MemberInfo member = GetFieldOrPropertyInfo(type, pathSegment);
                if (member is FieldInfo fieldInfo)
                {
                    value = fieldInfo.GetValue(source);
                    return true;
                }
                else if (member is PropertyInfo propertyInfo)
                {
                    value = propertyInfo.GetValue(source);
                    return true;
                }

                value = default;
                return false;
            }
        }

        private static bool SetFieldOrPropertyValue(object source, string pathSegment, object value)
        {
            Type type = source.GetType();
            if (type.IsArray && int.TryParse(pathSegment, NumberStyles.Integer, CultureInfo.InvariantCulture, out int index))
            {
                Array array = source as Array;
                array.SetValue(value, index);
                return true;
            }
            else
            {
                MemberInfo member = GetFieldOrPropertyInfo(type, pathSegment);
                if (member is FieldInfo fieldInfo)
                {
                    fieldInfo.SetValue(source, value);
                    return true;
                }
                else if (member is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(source, value);
                    return true;
                }

                return false;
            }
        }

        private static MemberInfo GetFieldOrPropertyInfo(Type type, string memberName)
        {
            MemberInfo[] members = type.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, flagAllAccessible);
            if (members != null && members.Length > 0)
            {
                return members[0];
            }

            throw new Exception($"Field or property not found: {memberName} in {type}");
        }
    }
}
