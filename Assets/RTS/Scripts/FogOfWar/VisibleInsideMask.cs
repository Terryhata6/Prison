/*using System.Collections.Generic;
using UnityEngine;

public class VisibleInsideMask : MonoBehaviour
{
    [HideInInspector] public Selectable selectable; 
    private List<GameObject> _enemies;
    private LayerMask _mask;

    private void Start()
    {
        selectable = GetComponent<Selectable>();
        if(SelectorRTS.Instance._team == selectable._team || selectable.gameObject.TryGetComponent<Resource>(out _))
        {
        }
        else
        {
            _enemies = new List<GameObject>();
            _mask = LayerMask.GetMask("Troop","Build");
            InvokeRepeating(nameof(CheckVisibility), 0f, 0.1f);
        }
    }

    public void CheckVisibility()
    {
        SetStateMesh(DetectEnemies());
    }

    public bool DetectEnemies()
    {
        _enemies.Clear();
        Vector3 capsuleStartPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z+selectable._distanceVision/2);
        Vector3 capsuleEndPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - selectable._distanceVision/2);
        Collider[] selectables = Physics.OverlapCapsule(capsuleStartPosition, capsuleEndPosition, selectable._distanceVision/2, _mask);
        foreach (Collider selectableEnemy in selectables)
        {
            if (selectableEnemy.gameObject.GetComponent<Selectable>()._team != selectable._team)
            {
                _enemies.Add(selectableEnemy.gameObject);
            }
        }

        if (_enemies.Count > 0)
        {
            return true;
        }
        return false;
    }

    private void SetStateMesh(bool state)
    {
        MeshRenderer mesh;
        if (TryGetComponent(out mesh))
        {
            mesh.enabled = state;
        }
        var MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in MeshRenderers)
        {
            meshRenderer.enabled = state;
        }
        selectable._healthBar.gameObject.SetActive(state);
        selectable.MiniatureMinimapGameObjects[0].SetActive(state);
    }
}*/