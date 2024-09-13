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

            if (!root.TryGetProperty("Description", out var descriptionElement))
            {
                throw new JsonException("Unable to find Description property in Race JSON");
            }

            string description = descriptionElement.GetString() ?? string.Empty;

            // Create the race object based on the name
            Race race = name switch
            {
                "Orc" => new Orc { Name = name, Description = description },
                "Human" => new Human { Name = name, Description = description },
                "Dwarf" => new Dwarf { Name = name, Description = description },
                "Elf" => new Elf { Name = name, Description = description },
                _ => throw new JsonException($"Unknown race: {name}")
            };


            // Verify that the description matches
            if (race.Description != description)
            {
                throw new JsonException($"Description mismatch for race {name}");
            }

            // Create a new instance with the correct bonuses
            race = race switch
            {
                Orc => new Orc
                {
                    Name = name,
                    Description = description,
                    StrengthBonus = root.TryGetProperty("StrengthBonus", out var s) ? s.GetInt32() : race.StrengthBonus,
                    ConstitutionBonus = root.TryGetProperty("ConstitutionBonus", out var c) ? c.GetInt32() : race.ConstitutionBonus
                },
                Human => new Human
                {
                    Name = name,
                    Description = description,
                    StrengthBonus = root.TryGetProperty("StrengthBonus", out var s) ? s.GetInt32() : race.StrengthBonus,
                    IntelligenceBonus = root.TryGetProperty("IntelligenceBonus", out var i) ? i.GetInt32() : race.IntelligenceBonus,
                    DexterityBonus = root.TryGetProperty("DexterityBonus", out var d) ? d.GetInt32() : race.DexterityBonus,
                    ConstitutionBonus = root.TryGetProperty("ConstitutionBonus", out var c) ? c.GetInt32() : race.ConstitutionBonus
                },
                Dwarf => new Dwarf
                {
                    Name = name,
                    Description = description,
                    StrengthBonus = root.TryGetProperty("StrengthBonus", out var s) ? s.GetInt32() : race.StrengthBonus,
                    ConstitutionBonus = root.TryGetProperty("ConstitutionBonus", out var c) ? c.GetInt32() : race.ConstitutionBonus
                },
                Elf => new Elf
                {
                    Name = name,
                    Description = description,
                    DexterityBonus = root.TryGetProperty("DexterityBonus", out var d) ? d.GetInt32() : race.DexterityBonus,
                    IntelligenceBonus = root.TryGetProperty("IntelligenceBonus", out var i) ? i.GetInt32() : race.IntelligenceBonus
                },
                _ => throw new JsonException($"Unknown race type: {race.GetType().Name}")
            };

            return race;
        }

        public override void Write(Utf8JsonWriter writer, Race value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteString("Description", value.Description);
            writer.WriteNumber("StrengthBonus", value.StrengthBonus);
            writer.WriteNumber("IntelligenceBonus", value.IntelligenceBonus);
            writer.WriteNumber("DexterityBonus", value.DexterityBonus);
            writer.WriteNumber("ConstitutionBonus", value.ConstitutionBonus);

            writer.WriteEndObject();
        }
    }
}