using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropPointBringSheep : MonoBehaviour
{
    public TextMeshProUGUI sheepText;
    float sheepBrought = 0;
    
    void Start()
    {
        sheepText = GetComponent<TextMeshProUGUI>();
        sheepText.text = sheepBrought.ToString();
    }

    public void UpdateSheepText(float sheep)
    {
      sheepBrought += sheep;
      sheepText.text = sheepBrought.ToString();
    }
}
