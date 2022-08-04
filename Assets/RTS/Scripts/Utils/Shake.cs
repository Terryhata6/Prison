/*
 * Jose Manuel Herrera Vera 
 * @projects_ia (Twitter)
 * https://josemhv.itch.io
 *
 * Shake v1.0
 * 
 * Date: 2021/10/25
 */
using UnityEngine;
using DG.Tweening;

public class Shake : MonoBehaviour
{
    public float Strengh = 1;
    public int Vibrato = 10;
    public float Randomness = 90;

    private void Start()
    {
        transform.DOShakeScale(1, Strengh, Vibrato, Randomness).SetLoops(-1).Play();
    }
}
