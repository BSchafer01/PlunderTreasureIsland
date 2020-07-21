using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{

    public Slider slider;


    public void SetCoin(int coin)
    {
        slider.value = coin;
    }

    public void SetMaxCoin(int coin)
    {
        slider.maxValue = coin;
    }

    public int GetCoin()
    {
        return (int)slider.value;
    }
    public int GetMaxCoin()
    {
        return (int)slider.maxValue;
    }
}
