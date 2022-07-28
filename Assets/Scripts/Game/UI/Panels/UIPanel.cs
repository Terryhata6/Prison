using System;
using UnityEngine;

namespace Game.UI.Panels
{
    public abstract class UIPanel : MonoBehaviour
    {
        private bool Enabled = false;
        public GameObject PanelHolder;

        public void Awake()
        {
            enabled = false;
            if (!PanelHolder) Debug.Log("PanelHolder does not exist", gameObject);
            PanelHolder.SetActive(false);
        }

        public virtual void Show()
        {
            if (Enabled)
            {
                return;
            }
            PanelHolder.SetActive(true);
            Enabled = true;

        }

        public virtual void Hide()
        {
            if (!Enabled)
            {
                return;
            }
            PanelHolder.SetActive(false);
            Enabled = false;
        }
    }
}