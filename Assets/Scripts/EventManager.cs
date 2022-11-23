using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void HungerFull();
    public static event HungerFull OnHungerFull;
    public delegate void GrabbedSheep();
    public static event HungerFull OnGrabSheep;
    public void HungerIsFull()
    {
        if (OnHungerFull != null)
        {
            OnHungerFull(); 
        }
    }

    public void GrabbedDatSheep()
    {
        if (OnGrabSheep != null)
        {
            OnGrabSheep();
        }
    }
}
