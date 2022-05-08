using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Helper
{
	public class DateTimeFormatConverter : JsonConverter<DateTime>
	{

		public DateTimeFormatConverter(string format)
		{
			Format = format;
		}

		private string Format { get; }

		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var stringValue = reader.GetString();
			return DateTime.ParseExact(stringValue, this.Format, CultureInfo.InvariantCulture);
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString(this.Format));
		}
	}


	public sealed class JsonDateTimeFormatAttribute : JsonConverterAttribute
	{
		private readonly string format;

		public JsonDateTimeFormatAttribute(string format)
		{
			this.format = format;
		}

		public string Format => this.format;

		public override JsonConverter? CreateConverter(Type typeToConvert)
		{
			return new DateTimeFormatConverter(this.format);
		}
	}

}
