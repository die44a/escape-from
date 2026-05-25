using System;
using _Project.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Runtime.Core.Props
{
    public class Lever : MonoBehaviour, IInteractable
    {
        [SerializeField] private Door targetDoor;
        [FormerlySerializedAs("InActivate")] public bool inActivate;
        public bool IsInteractable => true;
        public string GetInteractionLabel() => inActivate? "Деактивировать рычаг" : "Активировать рычаг";
        public SpriteRenderer Renderer { get; private set; }

        private Animator _animator;
        private static readonly int ActivatedKey = Animator.StringToHash("activated");
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Renderer = GetComponent<SpriteRenderer>();
        }
        
        public void Interact(GameObject initiator, Action onComplete)
        {
            onComplete?.Invoke();

            inActivate = !inActivate;
            _animator.SetBool(ActivatedKey, inActivate);
            
            targetDoor?.InteractFromLever(initiator, null);
        }
    }
}