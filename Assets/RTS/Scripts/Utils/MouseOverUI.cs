/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * MouseOverUI v1.0
 * 
 * Date: 2021/10/25
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Popup;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Popup.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Popup.SetActive(false);
    }

}
