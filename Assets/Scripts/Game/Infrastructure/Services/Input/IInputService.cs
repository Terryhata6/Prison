using System;
using Game.Infrastructure.Services;
using UnityEngine;

namespace Game.Logic.Services
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; set; }

        bool AttackButtonUp();

        Action<Vector3, float> OnStartTouch { get; set; }
        Action<Vector3, float> OnEndTouch { get; set; }
        Action<Vector3> OnMovedTouch { get; set; }
    }
}