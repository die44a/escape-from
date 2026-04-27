using System;

namespace _Project.Runtime.Player.Main
{
    public class PlayerStats
    {
        private int _bronzeCoins;
        private int _silverCoins;
        private int _goldCoins;

        public event Action<int, int, int> OnCoinsChanged;

        public void AddBronze()
        {
            _bronzeCoins++;
            OnCoinsChanged?.Invoke(_bronzeCoins, _silverCoins, _goldCoins);
        }

        public void AddSilver()
        {
            _silverCoins++;
            OnCoinsChanged?.Invoke(_bronzeCoins, _silverCoins, _goldCoins);
        }

        public void AddGold()
        {
            _goldCoins++;
            OnCoinsChanged?.Invoke(_bronzeCoins, _silverCoins, _goldCoins);
        }

        public int CalculateTotalScore()
            => _bronzeCoins * 1 + _silverCoins * 5 + _goldCoins * 10;
    }
}