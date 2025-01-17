﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvent : MonoBehaviour
{
    public delegate void PlayerLevelEventHandler(int currentExp, float fillAmount);
    public static event PlayerLevelEventHandler OnPlayerLevelChange;

    public delegate void PlayerHealthEventHandler(float currentHealth, float maxHealth);
    public static event PlayerHealthEventHandler OnPlayerHealthChanged;

    public delegate void PlayerManaEventHandler(float currentMana, float maxMana);
    public static event PlayerManaEventHandler OnPlayerManaChanged;

    public delegate void PlayerDieEventHandler();
    public static event PlayerDieEventHandler OnPlayerDie;

    public delegate void EnemyHealthChangeEventHandler(float fillAmount, int ID);
    public static event EnemyHealthChangeEventHandler OnEnemyHealthChange;

    public delegate void PlayerDragSomething(bool isDragging);
    public static event PlayerDragSomething OnPlayerDragging;

    public delegate void AddNewQuest(Quest quest);
    public static event AddNewQuest OnAddNewQuest;

    public delegate void NewClass();
    public static event NewClass OnAddNewClass;

    public delegate void ShowHint();
    public static event ShowHint OnShowHint;

    public static void ShowHintFKey()
    {
        if (OnShowHint != null)
            OnShowHint();
    }

    public static void NewGame()
    {
        if (OnAddNewClass != null)
            OnAddNewClass();
    }
    public static void AcceptQuest(Quest quest)
    {
        if (OnAddNewQuest != null)
            OnAddNewQuest(quest);
    }
    public static void HealthChanged(float currentHealth, float maxHealth)
    {
        if (OnPlayerHealthChanged != null)
            OnPlayerHealthChanged(currentHealth, maxHealth);
    }

    public static void ManaChanged(float currentMana, float maxMana)
    {
        if (OnPlayerManaChanged != null)
            OnPlayerManaChanged(currentMana, maxMana);
    }

    public static void PlayerLevelChange(int currentLvl, float fillAmount)
    {
        if (OnPlayerLevelChange != null)
        {
            OnPlayerLevelChange(currentLvl, fillAmount);
        }
    }

    public static void EnemyHPChange(float fillAmount, int ID)
    {
        if (OnEnemyHealthChange != null)
        {
            OnEnemyHealthChange(fillAmount, ID);
        }
    }

    public static void PlayerDie()
    {
        if (OnPlayerDie != null)
        {
            OnPlayerDie();
        }
    }

    public static void PlayerDragging( bool isDragging)
    {
        if (OnPlayerDragging != null)
        {
            OnPlayerDragging(isDragging);
        }
    }
}
