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
    bool WASDSent = false;
    bool grabbedSheep = false;
    bool disguisePut = false;

    
    

    public void HungerIsFull()
    {
        if (OnHungerFull != null)
        {
            OnHungerFull(); 
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
    
}
