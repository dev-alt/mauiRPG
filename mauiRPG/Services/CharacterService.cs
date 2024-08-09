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
        private readonly JsonSerializerOptions _jsonOptions;

        public CharacterService()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                Converters = { new RaceConverter(), new ClassConverter() },
                WriteIndented = true
            };
        }

        public void SaveCharacter(Character character)
        {
            List<Character> characters = LoadCharacters();
            characters.Add(character);
            SaveCharacters(characters);
        }

        public void SavePlayer(Player player)
        {
            List<Character> characters = LoadCharacters();
            characters.Add(player);
            SaveCharacters(characters);
        }

        public List<Character> LoadCharacters()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Character>>(json, _jsonOptions) ?? new List<Character>();
            }
            return new List<Character>();
        }

        public Player LoadPlayer(string playerName)
        {
            List<Character> characters = LoadCharacters();
            var player = characters.FirstOrDefault(c => c is Player p && p.Name == playerName) as Player;
            return player ?? throw new InvalidOperationException($"Player '{playerName}' not found.");
        }

        private void SaveCharacters(List<Character> characters)
        {
            var json = JsonSerializer.Serialize(characters, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
    }
}