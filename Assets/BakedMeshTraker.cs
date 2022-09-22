using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakedMeshTraker : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;

    public SkinnedMeshRenderer GetRenderer
    {
        get
        {
            if (_renderer == null)
            {
                _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            }

            return _renderer;
        }
    }
}
