using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheeps_Clothing : MonoBehaviour
{
    public bool isWearingClothing = false;
    public GameObject clothes;
    Wolf_Controller w_c;
    // Start is called before the first frame update
    void Start()
    {
        w_c = GetComponent<Wolf_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown("space"))
         {
            isWearingClothing = !isWearingClothing;
            if (isWearingClothing && !w_c.isCarryingSheep)
            {
                Stealth(true);
            }
            else
            {
               Stealth(false); 
            }
            
         }
    }

   
    public void Stealth(bool yesno)
    {
               clothes.SetActive(yesno);
                w_c.IsStealthed(yesno);
    }
}
