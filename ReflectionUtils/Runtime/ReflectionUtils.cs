using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AillieoUtils.CSReflectionUtils
{
    public static class ReflectionUtils
    {
        public static readonly BindingFlags flagAllAccessible = BindingFlags.Instance | BindingFlags.Static |
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
            return GetAllAccessibleFields(type, f => f.Name == name).FirstOrDefault();
        }

        public static PropertyInfo GetPropertyEx(Type type, string name)
        {
            return GetAllAccessibleProperties(type, p => p.Name == name).FirstOrDefault();
        }

        public static MethodInfo GetMethodEx(Type type, string name)
        {
            return GetAllAccessibleMethods(type, m => m.Name == name).FirstOrDefault();
        }

        public static IEnumerable<Type> FindSubTypes(Type baseType)
        {
            if (baseType.IsGenericType)
            {
                return GetAllTypes()
                        .Where(t => t.BaseType != null
                                    && t.BaseType.IsGenericType
                                    && t.BaseType.GetGenericTypeDefinition() == baseType);
            }
            else
            {
                return GetAllTypes()
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

        public static IEnumerable<KeyValuePair<T, Type>> GetAllTypesWithAttribute<T>()
            where T : Attribute
        {
            return GetAllTypes()
                .SelectMany(
                    t => t.GetCustomAttributes<T>(),
                    (type, attr) => new KeyValuePair<T, Type>(attr, type));
        }
    }
}
