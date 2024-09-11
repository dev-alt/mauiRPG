namespace mauiRPG.Services
{
    public interface ISettingsService
    {
        int GetMusicVolume();
        void SetMusicVolume(int volume);
        int GetSfxVolume();
        void SetSfxVolume(int volume);
        string GetDifficulty();
        void SetDifficulty(string difficulty);
        string GetTheme();
        void SetTheme(string theme);
    }

    public class SettingsService : ISettingsService
    {
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SfxVolume";
        private const string DifficultyKey = "Difficulty";
        private const string ThemeKey = "Theme";

        public int GetMusicVolume() => Preferences.Get(MusicVolumeKey, 50);
        public void SetMusicVolume(int volume) => Preferences.Set(MusicVolumeKey, volume);

        public int GetSfxVolume() => Preferences.Get(SfxVolumeKey, 50);
        public void SetSfxVolume(int volume) => Preferences.Set(SfxVolumeKey, volume);

        public string GetDifficulty() => Preferences.Get(DifficultyKey, "Normal");
        public void SetDifficulty(string difficulty) => Preferences.Set(DifficultyKey, difficulty);

        public string GetTheme() => Preferences.Get(ThemeKey, "Light");
        public void SetTheme(string theme) => Preferences.Set(ThemeKey, theme);
    }
}