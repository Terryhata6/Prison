using System.Collections;
using System.Collections.Generic;
using Game.Infrastructure.Services;
using Game.UI;
using UnityEngine;

public class CustomUIPauseButton : MonoBehaviour
{
    public void Pause()
    {
        AllServices.Container.Single<UIService>().SetState(UIState.Pause);
    }

    
    
}
