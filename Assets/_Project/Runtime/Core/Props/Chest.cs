using System;
using System.Collections;
using _Project.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private Collider2D interactableCollider;
        [SerializeField] private GameObject[] dropPrefabs;

        [Header("Loot flight")]
        [SerializeField] private float spawnHoldDuration = 0.12f;
        [SerializeField] private float flyDuration = 0.35f;
        [SerializeField] private float playerLandingSpread = 0.25f;
        [SerializeField] private float arcHeightMin = 0.3f;
        [SerializeField] private float arcHeightMax = 0.8f;

        [Header("Wall collision")]
        [SerializeField] private LayerMask obstacleLayers;
        [SerializeField] private float collisionCheckRadius = 0.25f;
        [SerializeField] private float wallPadding = 0.05f;

        public SpriteRenderer Renderer { get; private set; }
        public bool IsInteractable { get; private set; } = true;
        public string GetInteractionLabel() => "Открыть сундук";

        private bool _isBusy;
        private static readonly int Open = Animator.StringToHash("open");
        private Animator _animator;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            interactableCollider.isTrigger = false;

            if (obstacleLayers.value == 0)
                obstacleLayers = LayerMask.GetMask("Map");
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

            var chestPosition = (Vector2)transform.position;
            var playerPosition = (Vector2)initiator.transform.position;

            foreach (var prefab in dropPrefabs)
            {
                if (!prefab) continue;

                var item = Instantiate(prefab, chestPosition, Quaternion.identity);
                SetLootPickupEnabled(item, false);

                if (spawnHoldDuration > 0f)
                    yield return new WaitForSeconds(spawnHoldDuration);

                if (!item)
                    continue;

                var targetPosition = ResolveLandingPosition(chestPosition, playerPosition);
                yield return MoveItemToPlayer(item.transform, chestPosition, targetPosition);

                if (item)
                    SetLootPickupEnabled(item, true);
            }

            _isBusy = false;
            IsInteractable = !IsInteractable;
            onComplete.Invoke();
        }

        private Vector2 ResolveLandingPosition(Vector2 from, Vector2 playerPosition)
        {
            var target = playerPosition
                         + UnityEngine.Random.insideUnitCircle * playerLandingSpread;

            return TryResolvePath(from, target, out var resolved) ? resolved : playerPosition;
        }

        private IEnumerator MoveItemToPlayer(Transform item, Vector2 start, Vector2 target)
        {
            var t = 0f;
            var mid = (Vector3)(start + target) / 2f
                      + Vector3.up * UnityEngine.Random.Range(arcHeightMin, arcHeightMax);
            var previous = (Vector3)start;

            while (t < 1f)
            {
                if (!item)
                    yield break;

                t += Time.deltaTime / flyDuration;
                var smoothT = t * t * (3f - 2f * t);
                var a = Vector3.Lerp(start, mid, smoothT);
                var b = Vector3.Lerp(mid, target, smoothT);
                var next = Vector3.Lerp(a, b, smoothT);

                if (IsSegmentBlocked(previous, next))
                    break;

                item.position = next;
                previous = next;
                yield return null;
            }

            if (item)
                item.position = previous;
        }

        private bool TryResolvePath(Vector2 from, Vector2 to, out Vector2 resolved)
        {
            resolved = to;
            var delta = to - from;
            var distance = delta.magnitude;

            if (distance < 0.01f)
                return true;

            var direction = delta / distance;
            var hit = Physics2D.CircleCast(from, collisionCheckRadius, direction, distance, obstacleLayers);

            if (!hit)
                return true;

            resolved = hit.point - direction * (collisionCheckRadius + wallPadding);
            return true;
        }

        private bool IsSegmentBlocked(Vector2 from, Vector2 to)
        {
            var delta = to - from;
            var distance = delta.magnitude;
            if (distance < 0.001f)
                return false;

            return Physics2D.CircleCast(from, collisionCheckRadius, delta / distance, distance, obstacleLayers);
        }

        private static void SetLootPickupEnabled(GameObject loot, bool enabled)
        {
            if (!loot)
                return;

            if (loot.TryGetComponent<Coin>(out var coin))
                coin.SetPickupEnabled(enabled);
        }
    }
}
