﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomIdleAnim : StateMachineBehaviour
{
    float timeEachRandom = 5;
    float idleTime = 8;

    private const string BIDLE2_ANIM_TRIG = "bIdle2";
    private const string BIDLE3_ANIM_TRIG = "bIdle3";
    private const string BIDLE4_ANIM_TRIG = "bIdle4";
    private const string BIDLE5_ANIM_TRIG = "bIdle5";
    private const string BIDLE6_ANIM_TRIG = "bIdle6";

    string[] list = { BIDLE2_ANIM_TRIG, BIDLE3_ANIM_TRIG , BIDLE4_ANIM_TRIG, BIDLE5_ANIM_TRIG, BIDLE6_ANIM_TRIG };
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (idleTime <= 0)
        {
            RandomIdle(animator);
            idleTime = timeEachRandom;
        }
        else
        {
            idleTime -= Time.deltaTime;
        }
    }
    void RandomIdle(Animator animator)
    {
        System.Random random = new System.Random();
        int posIdle = random.Next(list.Length);
        string stringIdle = list[posIdle];
        animator.SetTrigger(stringIdle);
    }
}
