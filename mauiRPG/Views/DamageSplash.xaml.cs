using System;
using Microsoft.Maui.Controls;

namespace mauiRPG.Views
{
    public partial class DamageSplash : ContentView
    {
        public DamageSplash()
        {
            InitializeComponent();
        }

        public async Task ShowDamage(int damage)
        {
            DamageLabel.Text = damage.ToString();
            DamageLabel.Opacity = 1;
            await DamageLabel.TranslateTo(0, -50, 500, Easing.CubicOut);
            await DamageLabel.FadeTo(0, 500, Easing.CubicIn);
            DamageLabel.TranslationY = 0;
        }
    }
}