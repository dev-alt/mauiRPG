using System.Text.Json;
using System.Text.Json.Serialization;
using mauiRPG.Models;

namespace mauiRPG.Converters
{
    public class RaceConverter : JsonConverter<Race>
    {
        public override Race Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            if (!doc.RootElement.TryGetProperty("Name", out JsonElement nameElement))
            {
                throw new JsonException("Unable to find Name property.");
            }

            string name = nameElement.GetString() ?? string.Empty;

            return name switch
            {
                "Orc" => new Orc(),
                "Human" => new Human(),
                "Dwarf" => new Dwarf(),
                "Elf" => new Elf(),
                _ => throw new JsonException($"Unknown race: {name}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Race value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteEndObject();
        }
    }
}

