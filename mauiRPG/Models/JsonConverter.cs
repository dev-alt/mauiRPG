using System.Text.Json;
using System.Text.Json.Serialization;

namespace mauiRPG.Models
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

    public class ClassConverter : JsonConverter<Class>
    {
        public override Class Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                if (!doc.RootElement.TryGetProperty("Name", out JsonElement nameElement))
                {
                    throw new JsonException("Unable to find Name property.");
                }

                string name = nameElement.GetString() ?? string.Empty;

                return name switch
                {
                    "Warrior" => new Warrior(),
                    "Mage" => new Mage(),
                    "Rogue" => new Rogue(),
                    _ => throw new JsonException($"Unknown class: {name}")
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, Class value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteEndObject();
        }
    }
}