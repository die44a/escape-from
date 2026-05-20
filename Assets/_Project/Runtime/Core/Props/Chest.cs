using System;
using System.Collections;
using _Project.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private Collider2D interactableCollider;
        [SerializeField] private float dropDistance = 1f;
        [SerializeField] private float maxDropDistance = 1f;
        [SerializeField] private GameObject[] dropPrefabs;
        
        public SpriteRenderer Renderer { get; private set; }
        public bool IsInteractable { get; private set; } = true;
        public string GetInteractionLabel() => "Inspect Chest";
        
        private bool _isBusy;
        private static readonly int Open = Animator.StringToHash("open");
        private Animator _animator;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            interactableCollider.isTrigger = false;
        }

        public void Interact(GameObject initiator, Action onComplete)
        {
            if (_isBusy) return;
            StartCoroutine(InteractRoutine(initiator, onComplete));
        }

        private IEnumerator InteractRoutine(GameObject initiator, Action onComplete)
        {
            _isBusy = true;
            _animator.SetTrigger(Open);

            var direction = (transform.position - initiator.transform.position).normalized;
            var spawnPosition = transform.position + direction * dropDistance;

            foreach (var prefab in dropPrefabs)
            {
                if (!prefab) continue;
                var randomOffset = (Vector2)UnityEngine.Random.insideUnitCircle * maxDropDistance;
                var targetPosition = (Vector2)transform.position + randomOffset;
                var item = Instantiate(prefab, spawnPosition, Quaternion.identity);
                yield return StartCoroutine(MoveItem(item.transform, spawnPosition, targetPosition));
            }

            _isBusy = false;
            IsInteractable = !IsInteractable;
            onComplete.Invoke();
        }

        private IEnumerator MoveItem(Transform coin, Vector3 start, Vector3 target)
        {
            var t = 0f;
            const float duration = 0.35f;

            var mid = (start + target) / 2f + Vector3.up * UnityEngine.Random.Range(0.3f, 0.8f);

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                var smoothT = t * t * (3f - 2f * t);
                var a = Vector3.Lerp(start, mid, smoothT);
                var b = Vector3.Lerp(mid, target, smoothT);
                coin.position = Vector3.Lerp(a, b, smoothT);
                yield return null;
            }

            coin.position = target;
        }
    }
}