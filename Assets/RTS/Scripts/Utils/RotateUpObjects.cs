/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * RotateUp v1.0
 * 
 * Date: 2021/08/21
 */
using System.Collections.Generic;
using UnityEngine;

public class RotateUpObjects : Singleton<RotateUpObjects>
{
    public float speedMovement;
    public List<GameObject> objects;

    void Update()
    {
        foreach(GameObject obj in objects)
            obj.transform.Rotate(Vector3.up*speedMovement, Space.World);
    }
}
