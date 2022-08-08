using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarryPoses : MonoBehaviour
{
    private GameObject _currentBurry;
    public GameObject _sittingBarry;
    public GameObject _dancingBarry;
    public GameObject _pressingBarry;
    public GameObject _squatBarry;
    private int _sittingBarryTag = Animator.StringToHash("Sitting");
    private int _dancingBarryTag = Animator.StringToHash("Dancing");
    private int _pressingBarryTag = Animator.StringToHash("Pressing");
    private int _squatBarryTag = Animator.StringToHash("Squating");

    void Start()
    {
        _sittingBarry.SetActive(false);
        _dancingBarry.SetActive(false);
        _pressingBarry.SetActive(false);
        _squatBarry.SetActive(false);
        switch (Random.Range(0, 4))
        {
            case 0:
                _currentBurry = _sittingBarry;
                break;
            case 1:
                _currentBurry = _dancingBarry;
                break;
            case 2:
                _currentBurry = _pressingBarry;
                break;
            case 3:
                _currentBurry = _squatBarry;
                break;
            default:
                break;
        }
        _currentBurry.SetActive(true);
        _sittingBarry.GetComponent<Animator>().SetBool(_sittingBarryTag, true);   
        _dancingBarry.GetComponent<Animator>().SetBool(_dancingBarryTag, true);
        _pressingBarry.GetComponent<Animator>().SetBool(_pressingBarryTag, true);
        _squatBarry.GetComponent<Animator>().SetBool(_squatBarryTag, true);
        
        


        
    }
}