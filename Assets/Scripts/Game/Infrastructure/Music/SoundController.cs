using System;
using System.Collections;
using System.Collections.Generic;
using Game.Infrastructure.States;
using MoreMountains.Feedbacks;
using UnityEngine;

public class SoundController : MonoBehaviour, ISoundController
{
    [MMFInspectorButton("InitSoundsNames")]
    public bool buttonInitSoundNames;
    
    public List<SoundContainer> Sounds = new List<SoundContainer>();
    private Dictionary<String, MMF_Player> FastAccess = new Dictionary<string, MMF_Player>();
    
    private void Start()
    {
        foreach (var container in Sounds)
        {
            FastAccess.Add(container.SoundID, container.SoundPlayer);
        }
        PlaySound("MainTheme");
    }


    private void InitSoundsNames()
    {
        Sounds.Clear();
        foreach (var player in GetComponentsInChildren<MMF_Player>())
        {
            Sounds.Add(new SoundContainer(player.name, player));
        }
    }
    
    public void PlaySound(string id)
    {
        if (FastAccess.ContainsKey(id))
        {
            FastAccess[id].PlayFeedbacks();
        }
    }
}
[Serializable]
public class SoundContainer
{
    public string SoundID;
    public MMF_Player SoundPlayer;

    public SoundContainer(string soundId, MMF_Player soundPLayer)
    {
        SoundPlayer = soundPLayer;
        SoundID = soundId;
    }
}
