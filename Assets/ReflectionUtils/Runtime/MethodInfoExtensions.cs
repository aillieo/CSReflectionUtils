using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AillieoUtils.CSReflectionUtils
{
    public static class MethodInfoExtensions
    {
        public static string GetDeclaration(this MethodInfo methodInfo)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // accessibility
            if (methodInfo.IsPublic)
            {
                stringBuilder.Append("public ");
            }
            else if (methodInfo.IsFamily)
            {
                stringBuilder.Append("protected ");
            }
            else if (methodInfo.IsPrivate)
            {
                stringBuilder.Append("private ");
            }

            if (methodInfo.IsAssembly)
            {
                stringBuilder.Append("internal ");
            }

            // modifier
            if (methodInfo.IsStatic)
            {
                stringBuilder.Append("static ");
            }
            else if (methodInfo.IsAbstract)
            {
                stringBuilder.Append("abstract ");
            }
            else if (methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType)
            {
                if (methodInfo.IsFinal)
                {
                    stringBuilder.Append("sealed ");
                }

                stringBuilder.Append("override ");
            }
            else if (methodInfo.IsVirtual)
            {
                stringBuilder.Append("virtual ");
            }

            // async
            if (methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
            {
                stringBuilder.Append("async ");
            }

            // return type
            stringBuilder.Append(methodInfo.ReturnType.GetDeclaration());
            stringBuilder.Append(" ");

            // name
            stringBuilder.Append(methodInfo.Name);

            if (methodInfo.IsGenericMethod)
            {
                stringBuilder.Append("<");

                Type[] arguments = methodInfo.GetGenericArguments();

                stringBuilder.Append(string.Join(", ", arguments.Select(t => t.GetDeclaration())));

                stringBuilder.Append(">");
            }

            // parameters
            ParameterInfo[] parameters = methodInfo.GetParameters();
            stringBuilder.Append("(");
            stringBuilder.Append(string.Join(", ", parameters.Select(p => p.GetDeclaration())));
            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }
    }
}
