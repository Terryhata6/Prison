using System.Collections;
using System.Collections.Generic;
using Game.Infrastructure.Services;
using Game.UI;
using Game.UI.Interfaces;
using UnityEngine;

public class CustomUIPauseButton : MonoBehaviour
{
    public void Pause()
    {
        AllServices.Container.Single<IUIService>().SetState(UIState.Pause);
    }

    
    
}
