using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyWindow : MonoBehaviour
{
    public void StartGame(int difficulty)
    {
        FindObjectOfType<GameManager>().StartGame(difficulty);
    }
}
