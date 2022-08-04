/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * RotateUp v1.0
 * 
 * Date: 2021/08/21
 */
using UnityEngine;

public class RotateUp : MonoBehaviour
{
    public float speedMovement;

    private void Update()
    {
        transform.Rotate(Vector3.up*speedMovement, Space.World);
    }
}
