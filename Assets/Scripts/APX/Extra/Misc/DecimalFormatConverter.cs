using System;
using Newtonsoft.Json;

namespace APX.Extra.Misc
{
    public class DoubleFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(double);
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            var number = (double) value;
            if(number % 1 == 0)
            {
                writer.WriteValue((int) number);
            }
            else
            {
                writer.WriteValue(number);
            }
        }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();    
        }
    }
}
