using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public enum tutState
    {
        WASD,
        Animals,
        GrabSheep
    }
    public tutState currentState;
    [SerializeField] TextMeshProUGUI hints;
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = tutState.WASD;
        hints.text = "To move, use WASD or arrows";
        EventManager.OnGrabSheep += AnimalsToGrabSheep;
    }

    private void OnDestroy() {
        EventManager.OnGrabSheep -= AnimalsToGrabSheep;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == tutState.WASD)
        {
           
             StartCoroutine(WASDToAnimals());
           
        }
    }

    IEnumerator WASDToAnimals()
    {
        yield return new WaitForSeconds(1.5f);
        currentState = tutState.Animals;
        hints.text = "To eat an animal or grab a sheep, press G";

    }

    void AnimalsToGrabSheep()
    {
      currentState = tutState.GrabSheep;
      hints.text = "Drag that juicy sheep to the forest!"; 
    }
}
