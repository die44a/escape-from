using System;
using System.Collections;
using _Project.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public class Barrel : MonoBehaviour, IInteractable
    {
        public SpriteRenderer Renderer { get; private set; }
        private Animator _animator;
        [SerializeField] private Collider2D interactableCollider;

        [SerializeField] private GameObject goldPrefab;
        [SerializeField] private float dropDistance = 1f;

        [SerializeField] private Color highlightColor = Color.yellow;
        private Color _defaultColor;
        private bool _isHighlighted;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            // _animator = GetComponent<Animator>();
            _defaultColor = Renderer.color;
            interactableCollider.isTrigger = false;
        }

        public void Interact(GameObject initiator, Action onComplete)
        {
            onComplete?.Invoke();
            if (!IsInteractable) return;
            if (goldPrefab == null)
            {
                Debug.LogError("Gold Prefab is not assigned!");
                return;
            }
            var direction = (transform.position - initiator.transform.position).normalized;

            // 2. точка спавна рядом с бочкой
            var spawnPosition = transform.position + direction * dropDistance;

            // 3. симметричная точка (за игроком относительно бочки)
            var targetPosition = transform.position * 2f - initiator.transform.position;

            // 4. создаём монету
            var coin = Instantiate(goldPrefab, spawnPosition, Quaternion.identity);

            // 5. запускаем движение
            StartCoroutine(MoveCoin(coin.transform, targetPosition));

            ToggleHighlight();
            // var direction = (transform.position - initiator.transform.position).normalized;
            //
            // var spawnPosition = transform.position + direction * dropDistance;
            //
            // Instantiate(goldPrefab, spawnPosition, Quaternion.identity);
            ToggleHighlight();
        }
        private IEnumerator MoveCoin(Transform coin, Vector3 target)
        {
            float t = 0f;
            float duration = 0.35f;

            Vector3 start = coin.position;

            Vector3 mid = (start + target) / 2 + Vector3.up * 0.5f;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;

                float smoothT = t * t * (3f - 2f * t); // SmoothStep

                Vector3 a = Vector3.Lerp(start, mid, smoothT);
                Vector3 b = Vector3.Lerp(mid, target, smoothT);

                coin.position = Vector3.Lerp(a, b, smoothT);

                yield return null;
            }

            coin.position = target;
        }

        private void ToggleHighlight()
        {
            _isHighlighted = !_isHighlighted;
            Renderer.color = _isHighlighted ? highlightColor : _defaultColor;
            // IsInteractable = !IsInteractable;
        }

        public bool IsInteractable { get; private set; } = true;

        public string GetInteractionLabel() => "Inspect Barrel";
    }
}