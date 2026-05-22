using _Project.Runtime.Player.Controllers;
using UnityEngine;
using Zenject;
using TMPro;

public class HintView : MonoBehaviour
{
    [Inject] private PlayerController _player;

    [SerializeField] private float showDistance = 3f;
    [SerializeField] private float fadeSpeed = 5f;

    private SpriteRenderer[] _sprites;
    private TMP_Text[] _texts;

    private float _currentAlpha;
    private float _targetAlpha;

    private void Awake()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        _texts = GetComponentsInChildren<TMP_Text>();

        _currentAlpha = 0f;
        SetAlpha(_currentAlpha);
    }

    private void Update()
    {
        if (!_player)
            return;

        var sqrDist =
            (_player.transform.position - transform.position).sqrMagnitude;

        _targetAlpha = sqrDist <= showDistance * showDistance ? 1f : 0f;

        _currentAlpha = Mathf.Lerp(
            _currentAlpha,
            _targetAlpha,
            Time.deltaTime * fadeSpeed
        );

        SetAlpha(_currentAlpha);
    }

    private void SetAlpha(float alpha)
    {
        for (var i = 0; i < _sprites.Length; i++)
        {
            var c = _sprites[i].color;
            c.a = alpha;
            _sprites[i].color = c;
        }

        for (var i = 0; i < _texts.Length; i++)
        {
            var c = _texts[i].color;
            c.a = alpha;
            _texts[i].color = c;
        }
    }
}