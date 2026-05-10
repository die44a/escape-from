using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.15f;
    
    private readonly Color _hitColor = new (1f, 0.4f, 0.4f, 1f); 
    private readonly Color _normalColor = Color.white;

    public void ApplyDamage(float damage)
    {
        StartCoroutine(HitFlashRoutine());
    }
    
    private IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = _hitColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = _normalColor;
    }
}
