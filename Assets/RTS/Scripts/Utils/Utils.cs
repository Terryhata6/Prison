/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * Utils v2.0
 * 
 * Date: 2021/10/25
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils
{
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    //Get all Childs with tag
    public static List<GameObject> FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        List<GameObject> childsWithTag = new List<GameObject>();
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            foreach (GameObject child in FindGameObjectInChildWithTag(tr.gameObject, tag))
            {
                childsWithTag.Add(child);
            }
            if (tr.tag == tag)
            {
                childsWithTag.Add(tr.gameObject);
            }
        }
        return childsWithTag;
    }

    //SetActive List of GameObjects
    public static void ActiveGameObjects(List<GameObject> gameObjects, bool active)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(active);
        }
    }
/*
    //Get a Child with component
    public static GameObject FindGameObjectInChildWithComponent<T>(GameObject parent) where T : MonoBehaviour
    {
        GameObject child = null;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).TryGetComponent<Selectable>(out _))
            {
                child = parent.transform.GetChild(i).gameObject;
            }
        }
        return child;
    }*/
}
