using System;
using _Project.Runtime.Core.Configs;
using UnityEngine;
using Zenject;

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
            => BronzeCoins * _config.bronzeValue + 
               SilverCoins * _config.silverValue + 
               GoldCoins * _config.goldValue;
    }
}