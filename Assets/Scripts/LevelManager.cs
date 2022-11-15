using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int sheepStolenGoal;
    public int sheepStolenCounter = 0;
    public float countdown;

    private GameManager gameManager;
    private GameObject inGameUI;
    private TextMeshProUGUI sheepStolenCounterUI;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inGameUI = GameObject.Find("InGameUI");
        sheepStolenCounterUI = inGameUI.GetComponentInChildren<TextMeshProUGUI>();

        UpdateSheepCounterText();
        gameManager.UpdateLivesText();
        gameManager.UpdateHuntersCounterText();
        gameManager.huntersCounterOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sheepStolenCounter >= sheepStolenGoal)
        {
            gameManager.LevelWin();
            sheepStolenCounter = 0;
        }
    }

    public void SheepStolen(int i)
    {
        sheepStolenCounter += i;
        UpdateSheepCounterText();
    }

    void UpdateSheepCounterText()
    {
        sheepStolenCounterUI.text = $"{sheepStolenCounter} / {sheepStolenGoal}";
    }
}
