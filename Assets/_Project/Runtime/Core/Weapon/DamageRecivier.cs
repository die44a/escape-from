using UnityEngine;

public class DamageRecivier : MonoBehaviour, IDamageable
{
    public void ApplyDamage(float damage)
    {
        Debug.Log($"DamageRecivier: ApplyDamage({damage})");
    }
}