/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * FogScale v1.1
 * 
 * Date: 2021/10/25
 */
using UnityEngine;

[ExecuteInEditMode]
public class FogScale : MonoBehaviour
{
    [Range(1, 125)] [SerializeField] private float _sizeFactor = 1;
    [SerializeField] private Camera _mainCameraOfFog;
    [SerializeField] private Camera _secondaryCameraOfFog;
    [SerializeField] private Transform _fog;

    void Update()
    {
        _fog.localScale = new Vector3(_sizeFactor / 5, _fog.localScale.y, _sizeFactor / 5);
        _mainCameraOfFog.orthographicSize = _sizeFactor;
        _secondaryCameraOfFog.orthographicSize = _sizeFactor;

        if (Application.isPlaying) _fog.gameObject.SetActive(true);
        else _fog.gameObject.SetActive(_fog.gameObject.activeInHierarchy);

    }
}
