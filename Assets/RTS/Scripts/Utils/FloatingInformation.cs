/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * FloatingInformation v1.0
 * 
 * Date: 2021/10/25
 */
using UnityEngine;

public class FloatingInformation : MonoBehaviour
{
    public GameObject Popup;
    public Vector3 OffsetPosition;
    private GameObject PopupGO;

    private void OnMouseEnter()
    {
        PopupGO = Instantiate(Popup, transform.position + OffsetPosition, transform.rotation);
    }

    private void OnMouseExit()
    {
        Destroy(PopupGO);
    }
}
