using _Project.Runtime.Core.Camera;
using _Project.Runtime.Player.Controllers;
using UnityEngine;

namespace _Project.Runtime.Player.Services
{
    public class PlayerSpawnService
    {
        private readonly PlayerController _player;
        private readonly CameraPivotController _cameraPivot;

        public PlayerSpawnService(PlayerController player, CameraPivotController cameraPivot)
        {
            _player = player;
            _cameraPivot = cameraPivot;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Spawn(Vector3 position)
        {
            _player.ResetPlayer(position);
            
            Physics2D.SyncTransforms();

            Debug.Log($"[SpawnService] Player spawned at: {position}");
        }
    }
}