using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAnimals : MonoBehaviour
{
    bool isTouchingSheep;
    public bool isTouchingEatable;
    [HideInInspector] public bool isTouchingDropPoint;
    public Transform mouth;
    public int sheepValue = 1;
    // public float stealthCD = 1f;
    [HideInInspector] public Transform sheep;

    private WolfController wolfController;
    private SheepsClothing sheepsClothing;
    private LevelManager levelManager;
    WolfEmotes we;
    public IEatableAnimal eatableAnimal;
    

    void Start()
    {
        wolfController = GetComponent<WolfController>();
        sheepsClothing = GetComponent<SheepsClothing>();
        levelManager = FindObjectOfType<LevelManager>();
        we = GetComponent<WolfEmotes>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.G))
        {
            if(isTouchingSheep && !wolfController.isCarryingSheep)
            {
                sheep.SetParent(gameObject.transform, true);
                sheep.GetComponent<CircleCollider2D>().enabled = false;

                wolfController.IsCarryingSheep(true);
                sheepsClothing.Stealth(false);
                we.Emote(1);
                return;
            }
            else if (wolfController.isCarryingSheep)
            {
                sheep.GetComponent<CircleCollider2D>().enabled = true;
                sheep.SetParent(null);
                wolfController.IsCarryingSheep(false);
            }
            else if (isTouchingEatable)
            {
                eatableAnimal.IGotEaten();
            }
        }
    }

    public void DangleSheep(Transform whereMouth)
    {
       sheep.transform.position = whereMouth.position;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Sheep" && !wolfController.isCarryingSheep)
        {
           isTouchingSheep = true;

          
           sheep = other.gameObject.transform;

        }
         
    
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "DropPoint")
        {
            isTouchingDropPoint = true;

            if (wolfController.isCarryingSheep)
            {
                wolfController.IsCarryingSheep(false);
                Destroy(sheep.gameObject);           
                levelManager.SheepStolen(1);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
     {
        if (other.gameObject.tag == "Sheep" && !wolfController.isCarryingSheep)
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