using UnityEngine;
using Zenject;
using _Project.Runtime.Player.Controllers;

namespace _Project.Runtime.Core.UI.HUD
{
    public class ExitArrowView : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float edgeOffset = 50f;
        [SerializeField] private float smooth = 10f;

        [Inject] private PlayerController _playerController;
        [Inject] private IExitPointProvider _exitProvider;

        private Transform _player;
        private Transform _exit;
        private UnityEngine.Camera _cam;

        private void Start()
        {
            _player = _playerController.transform;
            _cam = UnityEngine.Camera.main;
        }

        private void Update()
        {
            _exit = _exitProvider.GetCurrentExit();

            if (!_exit || !_player)
            {
                arrow.gameObject.SetActive(false);
                return;
            }

            arrow.gameObject.SetActive(true);

            UpdateArrow();
        }

        private void UpdateArrow()
        {
            var screenPos = _cam.WorldToScreenPoint(_exit.position);

            var isOnScreen =
                screenPos.z > 0 &&
                screenPos.x > 0 &&
                screenPos.x < Screen.width &&
                screenPos.y > 0 &&
                screenPos.y < Screen.height;

            if (isOnScreen)
            {
                // режим “точно на цель”
                var worldDir = _exit.position - _player.position;
                worldDir.z = 0f;

                var angle = Mathf.Atan2(worldDir.y, worldDir.x) * Mathf.Rad2Deg;

                arrow.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

                // центрируем стрелку (или можно слегка подвести к центру HUD)
                var targetPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
                arrow.position = Vector2.Lerp(arrow.position, targetPos, Time.deltaTime * smooth);
            }
            else
            {
                // режим “по краю экрана”
                var dir = (Vector2)screenPos - new Vector2(Screen.width / 2f, Screen.height / 2f);
                dir.Normalize();

                var edgePos = (Vector2)screenPos;

                edgePos.x = Mathf.Clamp(edgePos.x, edgeOffset, Screen.width - edgeOffset);
                edgePos.y = Mathf.Clamp(edgePos.y, edgeOffset, Screen.height - edgeOffset);

                arrow.position = Vector2.Lerp(arrow.position, edgePos, Time.deltaTime * smooth);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                arrow.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            }
        }
    }
}