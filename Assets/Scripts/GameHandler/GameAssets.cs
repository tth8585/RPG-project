﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if (_i == null)
            {
                _i = Instantiate(Resources.Load<GameAssets>("Prefabs/GameAssets"));
            }
            return _i;
        }
    }

    //public AudioClip playerAttack;
    public SoundAudioClip[] soundAudioClipsArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
    //damage popup
    //public Transform pfDamagePopup;
}
