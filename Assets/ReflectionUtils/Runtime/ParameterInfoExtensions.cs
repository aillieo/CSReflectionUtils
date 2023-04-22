using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AillieoUtils.CSReflectionUtils
{
    public static class ParameterInfoExtensions
    {
        public static string GetDeclaration(this ParameterInfo parameterInfo)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // params
            if (Attribute.IsDefined(parameterInfo, typeof(ParamArrayAttribute)))
            {
                stringBuilder.Append("params ");
            }

            // in out ref
            if (parameterInfo.IsIn)
            {
                stringBuilder.Append("in ");
                stringBuilder.Append(parameterInfo.ParameterType.GetElementType().GetDeclaration());
            }
            else if (parameterInfo.IsOut)
            {
                stringBuilder.Append("out ");
                stringBuilder.Append(parameterInfo.ParameterType.GetElementType().GetDeclaration());
            }
            else if (parameterInfo.ParameterType.IsByRef)
            {
                stringBuilder.Append("ref ");
                stringBuilder.Append(parameterInfo.ParameterType.GetElementType().GetDeclaration());
            }
            else
            {
                stringBuilder.Append(parameterInfo.ParameterType.GetDeclaration());
            }

            stringBuilder.Append(" ");
            stringBuilder.Append(parameterInfo.Name);

            // = defaultValue
            if (parameterInfo.IsOptional)
            {
                if (parameterInfo.DefaultValue != Missing.Value)
                {
                    stringBuilder.Append(" = ");
                    stringBuilder.Append(parameterInfo.DefaultValue);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
