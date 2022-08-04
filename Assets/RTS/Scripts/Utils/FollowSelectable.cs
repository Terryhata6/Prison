/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * Follow Selectable v1.0
 * 
 * Date: 2021/08/21
 */
using UnityEngine;

public class FollowSelectable : MonoBehaviour
{
    private GameObject Selectable;
    private Vector3 offset;

    private void Start()
    {/*
        Selectable = Utils.FindGameObjectInChildWithComponent<Selectable>(transform.parent.gameObject);
        if(Selectable == null)
            Selectable = Utils.FindGameObjectInChildWithComponent<Selectable>(transform.parent.parent.gameObject);
        offset = transform.position - Selectable.transform.position;*/
    }
    
    private void Update()
    {/*
        if (Selectable.GetComponent<Selectable>().CanMove)
            transform.position = new Vector3(Selectable.transform.position.x + offset.x, Selectable.transform.position.y + offset.y, Selectable.transform.position.z + offset.z);
        else
            Destroy(this);*/
    }
}
