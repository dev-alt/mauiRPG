using mauiRPG.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace mauiRPG.Converters
{
    public class RaceConverter : JsonConverter<Race>
    {
        public override Race? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Invalid JSON structure for Race");
            }

            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("Name", out var nameElement))
            {
                throw new JsonException("Unable to find Name property in Race JSON");
            }

            string name = nameElement.GetString() ?? string.Empty;

            // Create the race object based on the name
            Race race = name switch
            {
                "Orc" => new Orc(),
                "Human" => new Human(),
                "Dwarf" => new Dwarf(),
                "Elf" => new Elf(),
                _ => throw new JsonException($"Unknown race: {name}")
            };

            // Populate common properties if they exist
            if (root.TryGetProperty("StrengthBonus", out var strengthElement))
                race.StrengthBonus = strengthElement.GetInt32();

            if (root.TryGetProperty("IntelligenceBonus", out var intelligenceElement))
                race.IntelligenceBonus = intelligenceElement.GetInt32();

            if (root.TryGetProperty("DexterityBonus", out var dexterityElement))
                race.DexterityBonus = dexterityElement.GetInt32();

            if (root.TryGetProperty("ConstitutionBonus", out var constitutionElement))
                race.ConstitutionBonus = constitutionElement.GetInt32();

            // Add any race-specific property deserialization here

            return race;
        }

        public override void Write(Utf8JsonWriter writer, Race value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteNumber("StrengthBonus", value.StrengthBonus);
            writer.WriteNumber("IntelligenceBonus", value.IntelligenceBonus);
            writer.WriteNumber("DexterityBonus", value.DexterityBonus);
            writer.WriteNumber("ConstitutionBonus", value.ConstitutionBonus);

            // Add any race-specific property serialization here

            writer.WriteEndObject();
        }
    }
}