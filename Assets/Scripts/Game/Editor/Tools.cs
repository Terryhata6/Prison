using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class Tools
    {
        [MenuItem("Tools/Clear prefs")]
        public static void CLearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}