using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepsClothing : MonoBehaviour
{
    public bool isWearingClothing = false;
    public bool isSeen = false;
    
    WolfController w_c;
    WolfEmotes w_e;
     SheepClothingSlider scs;
    
    
    
    void Start()
    {
        w_c = GetComponent<WolfController>();
        w_e = GetComponent<WolfEmotes>();
        scs = GetComponentInChildren<SheepClothingSlider>();
        
        
    }

    
    void Update()
    {
        
        
        
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) )
        {
            if ( !isSeen && !w_c.isCarryingSheep && !w_c.isStealthed)
            
            {
                scs.IsPuttingOnClothes(true);
                //Stealth(true);
            }
            else if (w_c.isStealthed && !scs.isDressing)
            {
                Stealth(false);
                
            }

        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (!w_c.isStealthed)
            {
               scs.IsPuttingOnClothes(false);
              
            }
        }

        
        
    }


    public void Stealth(bool yesno)
    {
        
        w_c.IsStealthed(yesno);
        if (yesno == false)
        {
          
          scs.IsPuttingOnClothes(false);
        }
        
    }
}
