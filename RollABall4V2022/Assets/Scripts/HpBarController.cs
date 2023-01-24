using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    private Slider _hpSlider;

    private void OnEnable()
    {
        _hpSlider = GetComponent<Slider>();
        
        PlayerObserverManager.OnPlayerHPChanged += UpdateHPBar;
    }
    
    private void OnDisable()
    {
        PlayerObserverManager.OnPlayerHPChanged -= UpdateHPBar;
    }

    private void UpdateHPBar(int value)
    {
        _hpSlider.value = value;
    }
}
