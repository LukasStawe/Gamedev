using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * The HealthBar UI. Has a Slider and Text to display the current Value and a reference to the player itself
 */
public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public TextMeshProUGUI healthValueText;

    //Maybe reference to StatManager instead ??
    public PlayerScript player;
    

    /**
     * @param maxHealth: The maximum Value of Players Health.
     * 
     * Sets the Maximum Value of Players-Health might change through Items (see Stats).
     */
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        healthValueText.text = slider.value + "/" + slider.maxValue;
    }
    
    /**
     * @param health: Current health of the Player.
     * 
     * Sets the current Health of the Player.
     */
    public void SetHealth(int health)
    {
        slider.value = health;
        healthValueText.text = slider.value + "/" + slider.maxValue;
    }
}
