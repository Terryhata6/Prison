/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * Singleton v1.0
 * 
 * Date: 2021/08/21
 */
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
            }
            return _instance;
        }
    }
}
