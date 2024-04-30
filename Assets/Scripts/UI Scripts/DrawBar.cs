using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawBar : MonoBehaviour
{
    [SerializeField]
    private GameObject sliderObject;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image fill;

    private Color standard = new Color(171,171,171);
    private Color green = Color.green;

    public void Toggle(bool value)
    {
        sliderObject.SetActive(value);
    }

    public void setValue(float value)
    {
        slider.value = value;
        if (value >= 75)
        {
            fill.color = green;
        } else
        {
            fill.color = standard;
        }
    }

}
