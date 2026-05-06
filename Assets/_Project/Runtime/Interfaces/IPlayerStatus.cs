using System;
using _Project.Runtime.Player.Main;
using UnityEngine;

public interface IPlayerStatus
{
    bool IsInvulnerableState { get; }
    Vector2 LastDirection { get; }
    event Action<PlayerState> OnStateChanged;
    PlayerState CurrentState { get; }
}