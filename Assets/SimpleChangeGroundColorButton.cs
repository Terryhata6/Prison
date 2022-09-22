using System;
using System.Collections;
using System.Collections.Generic;
using Game.Infrastructure.Music;
using Game.Infrastructure.Services;
using Game.UI;
using Game.UI.Interfaces;
using TMPro;
using UnityEngine;

public class SimpleChangeGroundColorButton : MonoBehaviour
{

    public TMP_Text copAmount;
    private void Awake()
    {
        if (copAmount)
        {
            copAmount.text = $"ChangeCopAmount, current: {PlayerPrefs.GetInt(CopController.CopAmountKey, 1)}";
        }
    }

    public void ChangeMaterial()
    {
        AllServices.Container.Single<IHighGroundVisualController>().ChangeMaterialToNextInList();
    }

    public void Unpause()
    {
        AllServices.Container.Single<IUIService>().SetState(UIState.Ingame);
    }

    private CopController CopController;
    public void ChangeCopAmount()
    {
        if (CopController == null)
        {
            CopController = FindObjectOfType<CopController>();
        }

        var currentAmount = PlayerPrefs.GetInt(CopController.CopAmountKey, 1);
        currentAmount += 1;
        if (currentAmount > 4)
        {
            currentAmount = 1;
        }
        
        if (CopController != null)
        {
            CopController.SetCopsAmount(currentAmount);
        }
        else
        {
            PlayerPrefs.SetInt(CopController.CopAmountKey, currentAmount);
        }

        if (copAmount)
        {
            copAmount.text = $"ChangeCopAmount, current: {currentAmount}";
        }
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
