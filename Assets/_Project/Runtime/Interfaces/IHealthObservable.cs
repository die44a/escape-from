using System;

public interface IHealthObservable
{
    event Action<float> OnHealthChanged;
    event Action OnDeath;
    float CurrentHealth { get; }
}