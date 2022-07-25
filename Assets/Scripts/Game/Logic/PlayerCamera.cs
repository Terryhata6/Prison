using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game.Hero;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;

    public void SetupCamera(HeroMove hero)
    {
        if (!_camera)
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }
        _camera.Follow = hero.transform;
        _camera.LookAt = hero.transform;
    }
}
