using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour
{
    public enum tutState
    {
        WASD,
        Animals,
        GrabSheep,
        DeliveredSheep,
        DogAndSheepsClothing,
        Gopher,
        GopherEaten,
        Level1Complete,
        TutComplete
    }
    public EventManager em;
    public tutState currentState;
    [SerializeField] TextMeshProUGUI hints;
    [SerializeField] GameObject turnOff;
    [SerializeField] GameObject hungerBarFill;
    [SerializeField] GameObject hungerBarBorder;
    bool barIsFlickering = false;
    public float hungerBarFlickerTime = 5f;
    public bool isOn = true;
    
    
    
    // Start is called before the first frame update
     private void Awake() 
    {
       
        EventManager.OnGrabSheep += AnimalsToGrabSheep;
        EventManager.OnTutStarted += TutStart;
        EventManager.OnWASD += StartWASDToAnimals;
        EventManager.OnHungerFull += StartGrabSheepToDeliveredSheep;
        EventManager.OnLevel0Complete += StartDeliveredToSheepsClothing;
        EventManager.OnDisguisePut += StartSheepsClothingToGopher;
        EventManager.OnGopherEaten += StartGopherToGopherEaten;
        EventManager.OnLevel1Complete += StartLevelOneComplete;
        EventManager.OnTutComplete += TutComplete;
        //SwitchWolfyFace();
    }

    private void OnDestroy() {
       Unsub();
    }

    public void Unsub()
    {
        EventManager.OnGrabSheep -= AnimalsToGrabSheep;
        EventManager.OnTutStarted -= TutStart;
        EventManager.OnWASD -= StartWASDToAnimals;
        EventManager.OnHungerFull -= StartGrabSheepToDeliveredSheep;
        EventManager.OnLevel0Complete -= StartDeliveredToSheepsClothing;
        EventManager.OnDisguisePut -= StartSheepsClothingToGopher;
        EventManager.OnGopherEaten -= StartGopherToGopherEaten;
        EventManager.OnLevel1Complete -= StartLevelOneComplete;
        EventManager.OnTutComplete -= TutComplete;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void TutStart()
    {
        
        turnOff.SetActive(true);
        hints.gameObject.SetActive(false);
        currentState = tutState.WASD;
        hints.text = "To move, use WASD or arrows";
        
    }

    void StartWASDToAnimals()
    {
       StopAllCoroutines(); 
      StartCoroutine(WASDToAnimals());
      // mb fading text
      
    }
    IEnumerator WASDToAnimals() //  EventManager.OnWASD
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

    

    IEnumerator FlickerHungerBar() // WASDToAnimals function
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
    void AnimalsToGrabSheep() // EventManager.OnGrabSheep
    {
      currentState = tutState.GrabSheep;
      hints.text = "Drag that juicy sheep to the forest!"; 
      
    }

    void StartGrabSheepToDeliveredSheep()
    {
        StopAllCoroutines();
        StartCoroutine(GrabSheepToDeliveredSheep());
        

    }
     
     IEnumerator GrabSheepToDeliveredSheep() //   EventManager.OnHungerFull
     {
        // disable space to exit lvl function temporarily
       currentState = tutState.DeliveredSheep;
        hints.text = "You earn points when you eat animals";
        // flicker points
        
        yield return new WaitForSeconds(1.5f);
        if (currentState == tutState.DeliveredSheep)
        hints.text = "You can escape to the forest when you're full";
        yield return new WaitForSeconds(2f);
        if (currentState == tutState.DeliveredSheep)
        hints.text = "Or stay and eat as many animals as you can!";
        // enable space to exit

     }


    void StartDeliveredToSheepsClothing()
    {
       currentState = tutState.DogAndSheepsClothing;
       StopAllCoroutines();
       StartCoroutine(DeliveredToSheepsClothing());
       


    }
    IEnumerator DeliveredToSheepsClothing() //        EventManager.OnLevel0Complete
    {
        hints.text = "Dogs can smell you... ";
        yield return new WaitForSeconds(3f);
        hints.text = "but they can't see you very well";
        yield return new WaitForSeconds(3f);
        hints.text = "To fool a dog, put on your disguise - hold <Space>!";

    }

    void StartSheepsClothingToGopher() 
    {
        StopAllCoroutines();
       StartCoroutine(SheepsClothingToGopher());
    }

    IEnumerator SheepsClothingToGopher()//   EventManager.OnDisguisePut
    {
       currentState = tutState.Gopher;
       hints.text = "Well done!";
       yield return new WaitForSeconds(2f);
       if (currentState == tutState.Gopher)
       hints.text = "A wolf isn't picky about food ";
       yield return new WaitForSeconds(3f);
       if (currentState == tutState.Gopher)
       hints.text = "A gopher might not be as yummy as a sheep...";
       // start flickering gopher contour
       yield return new WaitForSeconds(3f);
       if (currentState == tutState.Gopher)
       hints.text = "But edible";
       
       
       // hide text but flicker gopher
    }
 
    void StartGopherToGopherEaten()
    {
        //stop gopher flicker
        StopAllCoroutines();
       StartCoroutine(GopherToGopherEaten());
    }
     
     IEnumerator GopherToGopherEaten() //         EventManager.OnGopherEaten
     {
        currentState = tutState.GopherEaten;
        
        hints.text = "That's healthy food!";
        // flicker lives text + obj
        yield return new WaitForSeconds(2f);

        // stop flicker lives text + obj
       
     }

     void StartLevelOneComplete()
     {
        StopAllCoroutines();
       StartCoroutine(LevelOneComplete());
     }

     IEnumerator LevelOneComplete() // EventManager.OnLevel1Complete
     {
       currentState = tutState.Level1Complete;
       hints.text = "Farmers aren't particularly fond of wolves";
       // contour flicker farmer
       yield return new WaitForSeconds(3f);
       hints.text = "Farmers also come to check when their dogs bark loudly...";
       // fade text
     }

     void TutComplete()
     {
        currentState = tutState.TutComplete;
        Unsub();
        turnOff.SetActive(false);
        hints.gameObject.SetActive(false);
        
     }

     public void SwitchWolfyFace()
     {
        isOn = !isOn;
        if (!isOn)
        {
            hints.gameObject.SetActive(false);
           Color tmp = turnOff.GetComponent<Image>().color ;
           tmp = new Color(201,0,0); // red
           tmp.a = 0.5f;
           turnOff.GetComponent<Image>().color = tmp;
        }
         

         if (isOn)
         {
            hints.gameObject.SetActive(true);
             turnOff.GetComponent<Image>().color = new Color(0,255,0); // green
             WhatPartOfTut();
         }

           GameObject myEventSystem = GameObject.Find("EventSystem");
           myEventSystem .GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
         
     }

     void WhatPartOfTut()
     {
        //StopAllCoroutines();
        switch (currentState)
        {
             case tutState.WASD:
             em.WASDSent = false;
             break;     
            case tutState.Animals:
            { em.WASDSent = false;
                StartWASDToAnimals();
                }
            break;
            case tutState.GrabSheep:
            { 
                em.grabbedSheep = false;
                AnimalsToGrabSheep();
            }
            break;
            case tutState.DeliveredSheep:
            { 
                
                StartGrabSheepToDeliveredSheep();
            }
            break;
            case tutState.DogAndSheepsClothing: StartDeliveredToSheepsClothing();
            break;
            case tutState.Gopher:
            { 
                em.disguisePut = false;
                StartSheepsClothingToGopher();

            }
            break;
            case tutState.GopherEaten: StartGopherToGopherEaten();
            break;
            case tutState.Level1Complete: LevelOneComplete();
            break;
            case tutState.TutComplete: TutComplete();
            break;
        }
     }
}
