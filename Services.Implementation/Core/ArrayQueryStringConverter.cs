/*using System;
using System.ServiceModel.Dispatcher;

namespace Services.Implementation.Core
{
    public class ArrayQueryStringConverter : QueryStringConverter
    {
        public override bool CanConvert(Type type)
        {
            if (type.IsArray)
            {
                return base.CanConvert(type.GetElementType());
            }
            else
            {
                return base.CanConvert(type);
            }
        }

        public override object ConvertStringToValue(string parameter, Type parameterType)
        {
            if (parameterType.IsArray)
            {
                Type elementType = parameterType.GetElementType();
                string[] parameterList = parameter.Split(',');
                Array result = Array.CreateInstance(elementType, parameterList.Length);
                for (int i = 0; i < parameterList.Length; i++)
                {
                    result.SetValue(base.ConvertStringToValue(parameterList[i], elementType), i);
                }

                return result;
            }
            else
            {
                return base.ConvertStringToValue(parameter, parameterType);
            }
        }
    }
}
*/