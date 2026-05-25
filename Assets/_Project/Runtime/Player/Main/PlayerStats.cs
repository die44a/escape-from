using System;
using _Project.Runtime.Core.Configs;

namespace _Project.Runtime.Player.Main
{
    public class PlayerStats
    {
        private readonly CoinsConfig _config;

        public PlayerStats(CoinsConfig config) 
        {
            _config = config;
        }

        public int BronzeCoins { get; private set; }
        public int SilverCoins { get; private set; }
        public int GoldCoins { get; private set; }

        public event Action<int, int, int> OnCoinsChanged;

        public void AddBronze()
        {
            BronzeCoins++;
            Notify();
        }

        public void AddSilver()
        {
            SilverCoins++;
            Notify();
        }

        public void AddGold()
        {
            GoldCoins++;
            Notify();
        }

        public bool SpendBronze(int amount)
        {
            if (BronzeCoins < amount)
                return false;

            BronzeCoins -= amount;
            Notify();
            return true;
        }

        public bool SpendSilver(int amount)
        {
            if (SilverCoins < amount)
                return false;

            SilverCoins -= amount;
            Notify();
            return true;
        }

        public bool SpendGold(int amount)
        {
            if (GoldCoins < amount)
                return false;

            GoldCoins -= amount;
            Notify();
            return true;
        }

        public int CalculateTotalScore()
            => BronzeCoins * _config.bronzeValue +
               SilverCoins * _config.silverValue +
               GoldCoins * _config.goldValue;

        private void Notify()
        {
            OnCoinsChanged?.Invoke(BronzeCoins, SilverCoins, GoldCoins);
        }
    }
}