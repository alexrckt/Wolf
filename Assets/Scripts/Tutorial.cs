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
        GrabSheep,
        DeliveredSheep,
        DogAndSheepsClothing,
        Gopher
    }
    public EventManager em;
    public tutState currentState;
    [SerializeField] TextMeshProUGUI hints;
    [SerializeField] GameObject turnOff;
    [SerializeField] GameObject hungerBarFill;
    [SerializeField] GameObject hungerBarBorder;
    bool barIsFlickering = false;
    public float hungerBarFlickerTime = 5f;
    
    // Start is called before the first frame update
     private void Awake() 
    {
       
        EventManager.OnGrabSheep += AnimalsToGrabSheep;
        EventManager.OnTutStarted += TutStart;
        EventManager.OnWASD += StartWASDToAnimals;
        EventManager.OnHungerFull += GrabSheepToDeliveredSheep;
        EventManager.OnLevel0Complete += StartDeliveredToSheepsClothing;
        EventManager.OnDisguisePut += StartSheepsClothingToGopher;
    }

    private void OnDestroy() {
        EventManager.OnGrabSheep -= AnimalsToGrabSheep;
        EventManager.OnTutStarted -= TutStart;
        EventManager.OnWASD -= StartWASDToAnimals;
        EventManager.OnHungerFull -= GrabSheepToDeliveredSheep;
        EventManager.OnLevel0Complete -= StartDeliveredToSheepsClothing;
        EventManager.OnDisguisePut -= StartSheepsClothingToGopher;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void TutStart()
    {
        hints.gameObject.SetActive(true);
        turnOff.SetActive(true);
        currentState = tutState.WASD;
        hints.text = "To move, use WASD or arrows";
    }

    void StartWASDToAnimals()
    {
      StartCoroutine(WASDToAnimals());
      // mb fading text
      
    }
    IEnumerator WASDToAnimals()
    {
        yield return new WaitForSeconds(1.5f);
        currentState = tutState.Animals;
        hints.text = "You feel your stomach growling...";
        barIsFlickering = true;
        StartCoroutine(FlickerHungerBar());
        
        // maybe fading text
        yield return new WaitForSeconds(hungerBarFlickerTime);
        barIsFlickering = false;
        hungerBarBorder.SetActive(true);
        hungerBarFill.SetActive(true);
        if (currentState == tutState.Animals)
        {
          hints.text = "To eat an animal or grab a sheep, press G";
        }

    }

    void AnimalsToGrabSheep()
    {
      currentState = tutState.GrabSheep;
      hints.text = "Drag that juicy sheep to the forest!"; 
      
    }

    IEnumerator FlickerHungerBar()
    {
        while (barIsFlickering)
        {
            if (hungerBarBorder.activeInHierarchy && hungerBarFill.activeInHierarchy)
            {hungerBarBorder.SetActive(false);
            hungerBarFill.SetActive(false);}
            else
            {hungerBarBorder.SetActive(true);
            hungerBarFill.SetActive(true);}

            yield return new WaitForSeconds(0.5f);
        }
       
    }

    void GrabSheepToDeliveredSheep()
    {
        currentState = tutState.DeliveredSheep;
        hints.text = "Press <Space> to run away to forest!";
    }

    void StartDeliveredToSheepsClothing()
    {
       currentState = tutState.DogAndSheepsClothing;
       StartCoroutine(DeliveredToSheepsClothing());
       


    }
    IEnumerator DeliveredToSheepsClothing()
    {
        hints.text = "Dogs can smell you... ";
        yield return new WaitForSeconds(3f);
        hints.text = "but they can't see you very well";
        yield return new WaitForSeconds(3f);
        hints.text = "To fool a dog, put on your disguise - hold <Space>!";

    }

    void StartSheepsClothingToGopher()
    {
       StartCoroutine(SheepsClothingToGopher());
    }

    IEnumerator SheepsClothingToGopher()
    {
       currentState = tutState.Gopher;
       hints.text = "Well done!";
       yield return new WaitForSeconds(2f);
       hints.text = "Now, a wolf isn't picky in their food ";
       yield return new WaitForSeconds(2.5f);
       hints.text = "A gopher might not be as yummy as a sheep...";
       // start flickering gopher contour
       yield return new WaitForSeconds(2.5f);
       hints.text = "But nutritious";
       yield return new WaitForSeconds(2f);
       hints.text = "Let's eat one!";
    }
 
     
}
