using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetDashTimeValue(float value)
    {
        _slider.value = value;
    }
}
