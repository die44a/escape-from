using System;

public interface IHealthObservable
{
    event Action<float> OnHealthChanged;
    event Action OnDeath;
    event Action OnHit;
    float CurrentHealth { get; }
    float InvulnerabilityDuration { get; }
}