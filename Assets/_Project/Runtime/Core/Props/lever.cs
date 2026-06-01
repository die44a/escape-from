using System;
using System.Collections;
using _Project.Runtime.Interfaces;
using _Project.Services.Audio;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Runtime.Core.Props
{
    public class Lever : MonoBehaviour, IInteractable
    {
        [SerializeField] private Door targetDoor;
        [FormerlySerializedAs("InActivate")] public bool inActivate;
        public bool IsInteractable => !_isBusy;
        public string GetInteractionLabel() => inActivate ? "Деактивировать рычаг" : "Активировать рычаг";
        public SpriteRenderer Renderer { get; private set; }

        private Animator _animator;
        private static readonly int ActivatedKey = Animator.StringToHash("activated");

        private bool _isBusy;
        private IAudioService _audioService;

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Renderer = GetComponent<SpriteRenderer>();
        }

        public void Interact(GameObject initiator, Action onComplete)
        {
            if (_isBusy)
                return;

            _isBusy = true;

            onComplete?.Invoke();

            inActivate = !inActivate;
            _animator.SetBool(ActivatedKey, inActivate);

            _audioService?.PlaySound(SoundId.Lever);

            targetDoor?.InteractFromLever(initiator, null);

            StartCoroutine(ResetBusy());
        }

        private IEnumerator ResetBusy()
        {
            // лучше чуть больше чем длина анимации
            yield return new WaitForSeconds(0.4f);

            _isBusy = false;
        }
    }
}