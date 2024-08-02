using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using mauiRPG.Models;

namespace mauiRPG.Services
{
    class CharacterService
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
            Console.WriteLine($"Character saved to file: {character.Name}");
        }
    }
}
