using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerSlider : MonoBehaviour

{

    public Slider slider;
    
    public void AddFood(int food)
    {
      slider.value += food;
    }

    public void SetGoalHunger(int hungerGoal)
    {
       slider.maxValue = hungerGoal;
       slider.value = 0;
    }
}
