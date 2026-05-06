using System;
using _Project.Runtime.Player.Controllers;
using _Project.Runtime.Player.Main;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.Player
{
    public class PlayerStatusStub : IPlayerStatus {
        public bool IsInvulnerableState => false;
        public Vector2 LastDirection { get; }
        public event Action<PlayerState> OnStateChanged;
        public PlayerState CurrentState { get; }
    }
    
    [TestFixture]
    public class HealthMathTests
    {
        private HealthTimeController _controller;
        private GameObject _holder;

        [SetUp]
        public void SetUp()
        {
            _holder = new GameObject();
            _controller = _holder.AddComponent<HealthTimeController>();
            _controller.Construct(new PlayerStatusStub());
    
            var field = typeof(HealthTimeController)
                .GetField("invulnerabilityDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    
            if (field != null) 
                field.SetValue(_controller, 0f);

            _controller.AddTime(60f); 
        }

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(_holder);

        [Test]
        public void Calculate_StandardDamage_ReductionIsCorrect()
        {
            _controller.ApplyDamage(10.5f);
            Assert.AreEqual(49.5f, _controller.CurrentHealth, 0.001f);
        }

        [Test]
        public void Calculate_OverDamage_ResultIsExactlyZero()
        {
            _controller.ApplyDamage(100f);
            Assert.AreEqual(0f, _controller.CurrentHealth);
        }

        [Test]
        public void Calculate_HealPastMax_ResultIsExactlyMax()
        {
            _controller.ApplyDamage(10f); // 50
            _controller.AddTime(100f);   // 50 + 100, но лимит 60
            Assert.AreEqual(60f, _controller.CurrentHealth);
        }

        [Test]
        public void Calculate_WithModifier_DamageIsScaled()
        {
            var baseDamage = 5f;
            var modifier = 2.5f; 
            
            _controller.SetTimeModifier(modifier);
            _controller.ApplyDamage(baseDamage * modifier);

            // 60 - (5 * 2.5) = 47.5
            Assert.AreEqual(47.5f, _controller.CurrentHealth, 0.001f);
        }

        [Test]
        public void Calculate_ZeroModifier_HealthStaysStatic()
        {
            var initialHealth = _controller.CurrentHealth;
            
            _controller.SetTimeModifier(0f);
            _controller.ApplyDamage(10f * 0f);
    
            Assert.AreEqual(initialHealth, _controller.CurrentHealth);
        }

        [Test]
        public void Calculate_SmallValues_PrecisionCheck()
        {
            var smallStep = 0.000123f;
            _controller.ApplyDamage(smallStep);
            
            Assert.AreEqual(60f - smallStep, _controller.CurrentHealth, 0.000001f);
        }
        
        [Test]
        public void Calculate_ExtremelyHighDamage_StaysAtZero()
        {
            _controller.ApplyDamage(float.MaxValue);
    
            Assert.AreEqual(0f, _controller.CurrentHealth);
        }
        
        [Test]
        public void Calculate_AddZeroOrNegativeTime_DoesNotChangeHealth()
        {
            var healthBefore = _controller.CurrentHealth;
    
            _controller.AddTime(-50f);
            _controller.AddTime(0f);

            Assert.AreEqual(healthBefore, _controller.CurrentHealth);
        }
    }
}