using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void HungerFull();
    public static event HungerFull OnHungerFull;




   public void HungerIsFull()
   {
    if (OnHungerFull != null)
    {OnHungerFull();}
   }
}
