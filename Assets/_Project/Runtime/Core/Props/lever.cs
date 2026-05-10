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
        [FormerlySerializedAs("InActivate")] public bool inActivate;

        public bool IsInteractable => targetDoor != null;
        public string GetInteractionLabel() => "Use Lever";


        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        public void Interact(GameObject initiator, Action onComplete)
        {
            onComplete?.Invoke();
            if (targetDoor == null)
                return;

            inActivate = !inActivate;
            targetDoor.InteractFromLever(initiator, null);
        }
    }
}