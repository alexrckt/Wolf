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
    public bool fading = false;
    public bool fadeAndDisappear = false;
    GrabAnimals grabAnimals;
    
    
    
    // Start is called before the first frame update
     private void Awake() 
    {
       
        EventManager.OnGrabSheep += AnimalsToGrabSheep;
        EventManager.OnTutStarted += TutStart;
        EventManager.OnWASD += StartWASDToAnimals;
        EventManager.OnLevel0HungerFull += StartGrabSheepToDeliveredSheep;
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
        EventManager. OnLevel0HungerFull -= StartGrabSheepToDeliveredSheep;
        EventManager.OnLevel0Complete -= StartDeliveredToSheepsClothing;
        EventManager.OnDisguisePut -= StartSheepsClothingToGopher;
        EventManager.OnGopherEaten -= StartGopherToGopherEaten;
        EventManager.OnLevel1Complete -= StartLevelOneComplete;
        EventManager.OnTutComplete -= TutComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (hints.alpha >= 0.25f && !fadeAndDisappear)
             {hints.alpha -= 0.07f * Time.deltaTime;}
             
             if (fadeAndDisappear && hints.alpha > 0)
             {hints.alpha -= 0.1f * Time.deltaTime;}



             
        
        }
      
       
       
       


    }

    void TutStart()
    {
        
        turnOff.SetActive(true);
        //hints.gameObject.SetActive(false);
        currentState = tutState.WASD;
        hints.text = "To move, use WASD or arrows";
        Fading(true);

    }

    void StartWASDToAnimals()
    {
       StopAllCoroutines(); 
      StartCoroutine(WASDToAnimals());
      
      
    }
    IEnumerator WASDToAnimals() //  EventManager.OnWASD
    {
        yield return new WaitForSeconds(1.5f);
        currentState = tutState.Animals;
        hints.text = "You feel your stomach growling...";
        Fading(true);
        if (isOn)
        {
          barIsFlickering = true;
          StartCoroutine(FlickerHungerBar());
        }
       
        
        
        yield return new WaitForSeconds(hungerBarFlickerTime);
        barIsFlickering = false;
        hungerBarBorder.SetActive(true);
        hungerBarFill.SetActive(true);
        if (currentState == tutState.Animals)
        {
          hints.text = "To eat an animal or grab a sheep, press <G>";
          Fading(true);
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
      Fading(true);
      
    }

    void StartGrabSheepToDeliveredSheep()
    {
        StopAllCoroutines();
        StartCoroutine(GrabSheepToDeliveredSheep());
        

    }
     
     IEnumerator GrabSheepToDeliveredSheep() //   EventManager.OnHungerFull
     {
        if (isOn)
        {grabAnimals = FindObjectOfType<GrabAnimals>();
        grabAnimals.isTutBlockingExit = true; // blocking exit
        grabAnimals.isTouchingDropPoint = false;
        }
        

        currentState = tutState.DeliveredSheep;
        hints.text = "You earn points when you eat animals";
        Fading(true);
        
        yield return new WaitForSeconds(3f);
        
        hints.text = "Sheep will also gain you extra lives";
        
        yield return new WaitForSeconds(3f);
        
        hints.text = "You can escape to the forest when you're full";
        Fading(true);
        yield return new WaitForSeconds(3f);
        
        hints.text = "Or stay and eat as many animals as you can!";
        FadeAndDisappear(true);
        
        if (isOn)
        {
        grabAnimals.isTutBlockingExit = false; // unblocking exit
        yield return new WaitForFixedUpdate();
        if (grabAnimals.isTouchingDropPoint)
        grabAnimals.EnterForest(); 
        }
        

     }


    void StartDeliveredToSheepsClothing()
    {
       currentState = tutState.DogAndSheepsClothing;
       StopAllCoroutines();
       StartCoroutine(DeliveredToSheepsClothing());
       


    }
    IEnumerator DeliveredToSheepsClothing() //        EventManager.OnLevel0Complete
    {
         FadeAndDisappear(false);
        hints.text = "Dogs can smell you... ";
        Fading(true);
        yield return new WaitForSeconds(3f);
        hints.text = "but they can't see you very well";
        Fading(true);
        yield return new WaitForSeconds(3f);
        hints.text = "To fool a dog, put on your disguise - hold <Space>!";
        Fading(true);

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
       Fading(true);
       yield return new WaitForSeconds(2f);
       if (currentState == tutState.Gopher)
       hints.text = "A wolf isn't picky about food ";
       Fading(true);
       yield return new WaitForSeconds(3f);
       if (currentState == tutState.Gopher)
       hints.text = "A gopher might not be as yummy as a sheep...";
       Fading(true);
       
       yield return new WaitForSeconds(3f);
       if (currentState == tutState.Gopher)
       hints.text = "But is strangely energizing";
        FadeAndDisappear(true);
       em.GopherStartFlicker();
       
      
    }
 
    void StartGopherToGopherEaten()
    {
        
        StopAllCoroutines();
       StartCoroutine(GopherToGopherEaten());
    }
     
     IEnumerator GopherToGopherEaten() //         EventManager.OnGopherEaten
     {
         FadeAndDisappear(false);
        currentState = tutState.GopherEaten;
        
        hints.text = "You're running faster!";
        FadeAndDisappear(true);
        
        yield return new WaitForSeconds(2f);

       
       
     }

     void StartLevelOneComplete()
     {
        StopAllCoroutines();
       StartCoroutine(LevelOneComplete());
     }

     IEnumerator LevelOneComplete() // EventManager.OnLevel1Complete
     {
        FadeAndDisappear(false);
       currentState = tutState.Level1Complete;
       hints.text = "Farmers aren't particularly fond of wolves";
       Fading(true);
      
       yield return new WaitForSeconds(3f);
       hints.text = "Farmers also come to check when their dogs bark loudly...";
       FadeAndDisappear(true);
       
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

     void Fading(bool yesno)
     {
        hints.alpha = 1f;
        fading = yesno;
     }
     void FadeAndDisappear(bool yesno)
     {
        hints.alpha = 1f;
        fadeAndDisappear = yesno;
     }

     
}
