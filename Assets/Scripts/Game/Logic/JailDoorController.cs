using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailDoorController : MonoBehaviour
{
    public string DoorId = "";
    public BoxCollider _doorCollider;
    public Transform _doorTransform;
    public Vector3 _openedRotation;
    public Vector3 _closedRotation;
    
    
    public void Start()
    {
        if (PlayerPrefs.GetInt(DoorId, 0) > 0)
        {
            _doorTransform.Rotate(-_closedRotation);
            _doorCollider.enabled = false;
        }
        else
        {
            _doorTransform.rotation.SetLookRotation(_closedRotation);
            _doorCollider.enabled = true;
        }
    }

    
}
