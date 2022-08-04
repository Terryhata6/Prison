using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class AreaCircle : MonoBehaviour
{
    [Header("Range Zone")]
    [Range(0, 50)] public int segments = 50;
    public float width = 0.5f;
    public float radius;
    public Color lineColor;

    private LineRenderer _line;


    private void Start()
    {
        GetComponent<SphereCollider>().radius = radius;
        _line = GetComponent<LineRenderer>();

        _line.positionCount = segments + 1;
        _line.useWorldSpace = false;
        _line.startWidth = width;
        _line.endWidth = width;

        _line.enabled = true;

        SetRangeColor(lineColor);
        CreatePoints();
    }

    void SetRangeColor(Color c)
    {
        _line.startColor = c;
        _line.endColor = c;
        _line.material.color = c;
    }

    private void CreatePoints()
    {
        float x;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            _line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
}
