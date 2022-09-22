using System;
using System.Collections;
using System.Collections.Generic;
using Game.Infrastructure.Services;
using Game.Infrastructure.States;
using Game.Logic.Cop;
using MoreMountains.Feedbacks;
using UnityEngine;

public class CopController : MonoBehaviour
{
    public static readonly string CopAmountKey = "CopAmountKey";
    
    [MMFInspectorButton("FindAllCops")]public bool findAllCopsButton;
    public List<CopView> _cops = new List<CopView>();
    public int CopsAmount;

    private void Awake()
    {
        CopsAmount = PlayerPrefs.GetInt(CopAmountKey, 1);
    }

    public void FindAllCops()
    {
        _cops.Clear();
        
        _cops.AddRange(FindObjectsOfType<CopView>(true));
    }

    public void EnableCops()
    {
        AllServices.Container.Single<ISoundController>().PlaySound("Alarm");
        for (int i = 0; i < Mathf.Min(_cops.Count, CopsAmount); i++)
        {
            _cops[i].gameObject.SetActive(true);
        }
    }

    public int SetCopsAmount(int count)
    {
        CopsAmount = count;
        PlayerPrefs.SetInt(CopAmountKey, CopsAmount);
        return CopsAmount;
    }

    public int EnableCops(int amount)
    {
        SetCopsAmount(amount);
        EnableCops();
        return amount;
    }
}
