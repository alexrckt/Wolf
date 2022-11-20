using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerSlider : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        Paint(Color.yellow);
    }

    public void AddFood(int food)
    {
        slider.value += food;
        if(slider.value >= slider.maxValue)
        {
            Paint(Color.green);
        }
    }

    public void SetGoalHunger(int hungerGoal)
    {
        slider.maxValue = hungerGoal;
        slider.value = 0;
    }

    public void Paint(Color color)
    {
        transform.Find("Fill").GetComponent<Image>().color = color;
    }
}
