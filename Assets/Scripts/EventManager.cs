using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void HungerFull();
    public static event HungerFull OnHungerFull;
    

    public delegate void Tutorialka();

    public static event Tutorialka OnTutStarted;
    

    public static event Tutorialka OnWASD;
    
    
    public static event Tutorialka OnGrabSheep;
    
    public static event Tutorialka OnFirstBlood;
    public static event Tutorialka OnLevel0Complete;
    public static event Tutorialka OnDisguisePut;
    public static event Tutorialka OnGopherEaten;
    public static event Tutorialka OnLevel1Complete;
    public static event Tutorialka OnTutComplete;
    public static event Tutorialka OnGopherStartFlicker;

    public bool WASDSent = false;
    public bool grabbedSheep = false;
    public bool disguisePut = false;
    public bool level0HungerIsFull = false;
    

    
    

    public void HungerIsFull()
    {
        if (OnHungerFull != null && !level0HungerIsFull)
        {
            OnHungerFull(); 
            level0HungerIsFull = true;
        }
    }
    
    public void WASD()
    {
        if (OnWASD != null && !WASDSent)
        {
            OnWASD(); 
            WASDSent = true;
        }
    }
   
    


    public void GrabbedDatSheep()
    {
        if (OnGrabSheep != null && !grabbedSheep)
        {
            OnGrabSheep();
            grabbedSheep = true;
        }
    }
    

    public void TutStart()
    {
        if (OnTutStarted != null)
        {
            OnTutStarted();
        }
    }

    public void FirstBlood()
    {
        if (OnFirstBlood != null)
        {
            OnFirstBlood();
        }
    }

    public void Level0Complete()
    {
        if (OnLevel0Complete != null)
        {
            OnLevel0Complete();
        }
    }

    public void DisguisePut()
    {
         if (OnDisguisePut != null && !disguisePut)
        {
            OnDisguisePut();
            disguisePut = true;
        }
    }

    public void GopherEaten()
    {
         if (OnGopherEaten != null )
        {
            OnGopherEaten();
            
        }
    }

    public void Level1Complete()
    {
         if (OnLevel1Complete != null )
        {
            OnLevel1Complete();
            
        }
    }
    

    public void TutComplete()
    {
         if (OnTutComplete != null )
        {
            OnTutComplete();
            
        }
    }

    public void GopherStartFlicker()
    {
       if (OnGopherStartFlicker != null )
        {
            OnGopherStartFlicker();
            
        } 
    }

    
    
}
