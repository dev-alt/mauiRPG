using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using mauiRPG.Converters;
using mauiRPG.Models;

namespace mauiRPG.Services
{
    public class CharacterService
    {
        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "characters.json");
        private readonly JsonSerializerOptions _jsonOptions;
        private const int MaxCharacters = 5;

        public CharacterService()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                Converters = { new RaceConverter(), new CharacterConverter() },
                WriteIndented = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };
        }

        public bool SaveCharacter(Character character, bool @override = false)
        {
            List<Character> characters = LoadCharacters();

            if (characters.Count >= MaxCharacters && !@override)
            {
                return false;
            }

            var existingIndex = characters.FindIndex(c => c.Name == character.Name);
            if (existingIndex != -1)
            {
                if (@override)
                {
                    characters[existingIndex] = character;
                }
                else
                {
                    return false;
                }
            }
            else if (characters.Count < MaxCharacters)
            {
                characters.Add(character);
            }
            else
            {
                return false;
            }

            SaveCharacters(characters);
            return true;
        }

        public List<Character> LoadCharacters()
        {
            if (!File.Exists(_filePath)) return [];
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Character>>(json, _jsonOptions) ?? [];
        }

        public Player? LoadPlayer(string playerName)
        {
            List<Character> characters = LoadCharacters();
            return characters.FirstOrDefault(c => c is Player p && p.Name == playerName) as Player;
        }

        public bool DeleteCharacter(string characterName)
        {
            List<Character> characters = LoadCharacters();
            var character = characters.FirstOrDefault(c => c.Name == characterName);
            if (character != null)
            {
                characters.Remove(character);
                SaveCharacters(characters);
                return true;
            }

            return false;
        }

        private void SaveCharacters(List<Character> characters)
        {
            var json = JsonSerializer.Serialize(characters, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
    }






    public class CharacterConverter : JsonConverter<Character>
    {
        public override Character? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Invalid JSON structure for Character");
            }

            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("Race", out var raceElement))
            {
                // This is a Player
                var player = new Player
                {
                    Name = root.GetProperty("Name").GetString() ?? "",
                    MaxHealth = root.GetProperty("MaxHealth").GetInt32(),
                    CurrentHealth = root.GetProperty("CurrentHealth").GetInt32(),
                    Attack = root.GetProperty("Attack").GetInt32(),
                    Defense = root.GetProperty("Defense").GetInt32(),
                    Strength = root.GetProperty("Strength").GetInt32(),
                    Intelligence = root.GetProperty("Intelligence").GetInt32(),
                    Dexterity = root.GetProperty("Dexterity").GetInt32(),
                    Constitution = root.GetProperty("Constitution").GetInt32(),
                    Level = root.GetProperty("Level").GetInt32(),
                    Experience = root.GetProperty("Experience").GetInt32(),
                    Gold = root.GetProperty("Gold").GetInt32(),
                    IsDefending = root.GetProperty("IsDefending").GetBoolean(),
                    Race = JsonSerializer.Deserialize<Race>(raceElement.GetRawText(), options)
                           ?? throw new JsonException("Failed to deserialize Race"),
                    EquippedItems = root.TryGetProperty("EquippedItems", out var equippedItemsElement)
                        ? JsonSerializer.Deserialize<Dictionary<EquipmentSlot, Equipment>>(
                              equippedItemsElement.GetRawText(), options)
                          ?? new Dictionary<EquipmentSlot, Equipment>()
                        : new Dictionary<EquipmentSlot, Equipment>(),
                    Inventory = root.TryGetProperty("Inventory", out var inventoryElement)
                        ? JsonSerializer.Deserialize<ObservableCollection<Item>>(inventoryElement.GetRawText(), options)
                          ?? new ObservableCollection<Item>()
                        : new ObservableCollection<Item>(),
                    ActiveQuests = root.TryGetProperty("ActiveQuests", out var activeQuestsElement)
                        ? JsonSerializer.Deserialize<List<Quest>>(activeQuestsElement.GetRawText(), options)
                          ?? new List<Quest>()
                        : new List<Quest>(),
                    CompletedQuests = root.TryGetProperty("CompletedQuests", out var completedQuestsElement)
                        ? JsonSerializer.Deserialize<List<Quest>>(completedQuestsElement.GetRawText(), options)
                          ?? new List<Quest>()
                        : new List<Quest>()
                };

                return player;
            }
            else
            {
                // This is a regular Character
                return new Character
                {
                    Name = root.GetProperty("Name").GetString() ?? "",
                    MaxHealth = root.GetProperty("MaxHealth").GetInt32(),
                    CurrentHealth = root.GetProperty("CurrentHealth").GetInt32(),
                    Attack = root.GetProperty("Attack").GetInt32(),
                    Defense = root.GetProperty("Defense").GetInt32(),
                    Strength = root.GetProperty("Strength").GetInt32(),
                    Intelligence = root.GetProperty("Intelligence").GetInt32(),
                    Dexterity = root.GetProperty("Dexterity").GetInt32(),
                    Constitution = root.GetProperty("Constitution").GetInt32(),
                    Level = root.GetProperty("Level").GetInt32()
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, Character value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Name", value.Name);
            writer.WriteNumber("MaxHealth", value.MaxHealth);
            writer.WriteNumber("CurrentHealth", value.CurrentHealth);
            writer.WriteNumber("Attack", value.Attack);
            writer.WriteNumber("Defense", value.Defense);
            writer.WriteNumber("Strength", value.Strength);
            writer.WriteNumber("Intelligence", value.Intelligence);
            writer.WriteNumber("Dexterity", value.Dexterity);
            writer.WriteNumber("Constitution", value.Constitution);
            writer.WriteNumber("Level", value.Level);

            if (value is Player player)
            {
                writer.WritePropertyName("Race");
                JsonSerializer.Serialize(writer, player.Race, options);

                writer.WriteNumber("Experience", player.Experience);
                writer.WriteNumber("Gold", player.Gold);
                writer.WriteBoolean("IsDefending", player.IsDefending);

                writer.WritePropertyName("EquippedItems");
                JsonSerializer.Serialize(writer, player.EquippedItems, options);

                writer.WritePropertyName("Inventory");
                JsonSerializer.Serialize(writer, player.Inventory, options);

                writer.WritePropertyName("ActiveQuests");
                JsonSerializer.Serialize(writer, player.ActiveQuests, options);

                writer.WritePropertyName("CompletedQuests");
                JsonSerializer.Serialize(writer, player.CompletedQuests, options);
            }

            writer.WriteEndObject();
        }
    }
}
