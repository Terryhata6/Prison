/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * Billboards v1.0
 * 
 * Date: 2021/08/21
 */
using UnityEngine;

public class Billboards : MonoBehaviour
{
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
