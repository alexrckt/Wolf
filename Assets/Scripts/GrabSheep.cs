using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSheep : MonoBehaviour
{
    Wolf_Controller wc;
    Sheeps_Clothing sc;
    DropPoint_BringSheep dpBSheep;
    bool isTouchingSheep;
    [HideInInspector] public bool isTouchingDropPoint;
    public Transform mouth;
    public float sheepValue = 1f;
    [HideInInspector] public Transform sheep;
    
    void Start()
    {
        wc = GetComponent<Wolf_Controller>();
        sc = GetComponent<Sheeps_Clothing>();
        dpBSheep = FindObjectOfType<DropPoint_BringSheep>();

    }

    
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.G))
        {
          if(isTouchingSheep && !wc.isCarryingSheep)
          {
            sheep.SetParent(gameObject.transform, true);
            sheep.GetComponent<CircleCollider2D>().enabled = false;
            
            
            wc.IsCarryingSheep(true);
            sc.Stealth(false);
            return;
          }
          if (wc.isCarryingSheep)
          {
            sheep.GetComponent<CircleCollider2D>().enabled = true;
            sheep.SetParent(null);
            
            wc.IsCarryingSheep(false);
            
          }
        }
    }

    public void DangleSheep(Transform whereMouth)
    {
       sheep.transform.position = whereMouth.position;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Sheep")
        {
           isTouchingSheep = true;

          
           sheep = other.gameObject.transform;

        }
         
    
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "DropPoint")
        {
          isTouchingDropPoint = true;

          if (wc.isCarryingSheep)
        {
          wc.IsCarryingSheep(false);
           Destroy(sheep.gameObject);           
           dpBSheep.UpdateSheepText(sheepValue);

        }
        }

        
    }

     private void OnCollisionExit2D(Collision2D other) 
     {
        if (other.gameObject.tag == "Sheep" && !wc.isCarryingSheep)
        {
           isTouchingSheep = false;
           

        }
        
        
    }

    private void OnTriggerExit2D(Collider2D other) {

        if (other.gameObject.tag == "DropPoint")
        {
          isTouchingDropPoint = false;
        }
    }
}
