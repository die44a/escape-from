using System;
using _Project.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Runtime.Core.Props
{
    public class Lever : MonoBehaviour, IInteractable
    {
        public SpriteRenderer Renderer { get; private set; }

        [SerializeField] private Door targetDoor;

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        public bool IsInteractable => targetDoor != null;

        public string GetInteractionLabel() => "Use Lever";

        public void Interact(GameObject initiator, Action onComplete)
        {
            onComplete?.Invoke();

            if (targetDoor == null)
            {
                Debug.Log("Target Door is null");
                return;
            }

            inActivate = !inActivate;
            targetDoor.InteractFromLever(initiator, null);
        }

        [FormerlySerializedAs("InActivate")] public bool inActivate;
    }
}