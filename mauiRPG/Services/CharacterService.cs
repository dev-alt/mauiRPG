using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using mauiRPG.Models;

namespace mauiRPG.Services
{
    public class CharacterService
    {
        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "characters.json");

        public void SaveCharacter(Character character)
        {
            List<Character> characters;

            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                characters = JsonSerializer.Deserialize<List<Character>>(json) ?? new List<Character>();
            }
            else
            {
                characters = new List<Character>();
            }

            characters.Add(character);
            var newJson = JsonSerializer.Serialize(characters);
            File.WriteAllText(_filePath, newJson);
        }

        public void SavePlayer(Player player)
        {
            Character character = new Character
            {
                Name = player.Name,
                Health = player.Health,
                Level = player.Level,
                Strength = player.Strength,
                Intelligence = player.Intelligence,
                Dexterity = player.Dexterity,
                Constitution = player.Constitution
            };

            SaveCharacter(character);
        }

        public List<Character> LoadCharacters()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Character>>(json) ?? new List<Character>();
            }
            return new List<Character>();
        }

        private void SaveCharacters(List<Character> characters)
        {
            var json = JsonSerializer.Serialize(characters);
            File.WriteAllText(_filePath, json);
        }
    }
}