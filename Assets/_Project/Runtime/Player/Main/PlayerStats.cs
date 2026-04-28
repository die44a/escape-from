using System;

namespace _Project.Runtime.Player.Main
{
    public class PlayerStats
    {
        public int BronzeCoins { get; private set; }
        public int SilverCoins { get; private set; }
        public int GoldCoins { get; private set; }
        
        public event Action<int, int, int> OnCoinsChanged;

        public void AddBronze()
        {
            BronzeCoins++;
            OnCoinsChanged?.Invoke(BronzeCoins, SilverCoins, GoldCoins);
        }

        public void AddSilver()
        {
            SilverCoins++;
            OnCoinsChanged?.Invoke(BronzeCoins, SilverCoins, GoldCoins);
        }

        public void AddGold()
        {
            GoldCoins++;
            OnCoinsChanged?.Invoke(BronzeCoins, SilverCoins, GoldCoins);
        }

        public int CalculateTotalScore()
            => BronzeCoins * 1 + SilverCoins * 5 + GoldCoins * 10;
    }
}