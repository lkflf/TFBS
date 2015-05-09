﻿using UnityEngine;
using UnityEngine.UI;

public class HealthBar : BaseComponent
{
    Slider slider;

    protected override void OnStart()
    {
        slider = GetComponent<Slider>();
    }

    #region EventManagement
    PlayerDamage playerDamage;

    protected override void HookUpEvents()
    {
        playerDamage = GameObject.FindObjectOfType<PlayerDamage>();
        playerDamage.HealthPointsChanged += playerDamage_HealthPointsChanged;

        playerDamage.OnInitialized(playerDamage_Initialized);
    }

    protected override void UnHookEvents()
    {
        if (playerDamage != null)
            playerDamage.HealthPointsChanged -= playerDamage_HealthPointsChanged;
    }
    #endregion

    #region EventHandlers
    void playerDamage_HealthPointsChanged(int value, int delta)
    {
        if (value > 100)
            slider.value = 100;
        else
            slider.value = value;
    }

    void playerDamage_Initialized()
    {
        playerDamage_HealthPointsChanged(playerDamage.HealthPoints, 0);
    }
    #endregion
}
