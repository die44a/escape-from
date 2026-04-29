using _Project.Runtime.Core.Configs;
using _Project.Runtime.Player.Main;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.Player
{
    [TestFixture]
    public class PlayerStatesTests
    {
        private PlayerStats _stats;
        private CoinsConfig _coinsConfig;

        [SetUp]
        public void SetUp()
        {
            _coinsConfig = ScriptableObject.CreateInstance<CoinsConfig>();

            _stats = new PlayerStats(_coinsConfig);
        }

        [Test]
        public void AddBronze_IncrementsValue()
        {
            _stats.AddBronze();
            Assert.AreEqual(1, _stats.BronzeCoins);
        }

        [Test]
        public void AddSilver_IncrementsValue()
        {
            _stats.AddSilver();
            Assert.AreEqual(1, _stats.SilverCoins);
        }

        [Test]
        public void AddGold_IncrementsValue()
        {
            _stats.AddGold();
            Assert.AreEqual(1, _stats.GoldCoins);
        }

        [Test]
        public void CalculateTotalScore_CalculatesDefaultCorrectly()
        {
            _stats.AddBronze();
            _stats.AddSilver();
            _stats.AddGold();
            var totalScore = _stats.CalculateTotalScore();
            var expected = _coinsConfig.bronzeValue + _coinsConfig.silverValue + _coinsConfig.goldValue;
            Assert.AreEqual(expected, totalScore);
        }
        
        [Test]
        public void OnCoinsChanged_FiresCorrectValues_WhenBronzeAdded()
        {
            var firedBronze = -1;
            _stats.OnCoinsChanged += (b, s, g) => firedBronze = b;

            _stats.AddBronze();

            Assert.AreEqual(1, firedBronze);
        }
        
        [Test]
        public void AddGold_DoesNotAffectOtherCoins()
        {
            _stats.AddGold();

            Assert.AreEqual(1, _stats.GoldCoins);
            Assert.AreEqual(0, _stats.SilverCoins);
            Assert.AreEqual(0, _stats.BronzeCoins);
        }
        
        [Test]
        public void InitialTotalScore_IsZero()
        {
            Assert.AreEqual(0, _stats.CalculateTotalScore());
        }
        
        [Test]
        public void MultipleCoins_SumCorrectly()
        {
            for (var i = 0; i < 100; i++) 
                _stats.AddBronze();
    
            Assert.AreEqual(100, _stats.BronzeCoins);
        }
        
        [Test]
        public void CalculateTotalScore_UpdatesWhenConfigChanges()
        {
            _stats.AddGold(); // 1 золото
            _coinsConfig.goldValue = 10;
            Assert.AreEqual(10, _stats.CalculateTotalScore());

            _coinsConfig.goldValue = 20;
            Assert.AreEqual(20, _stats.CalculateTotalScore());
        }
    }
}