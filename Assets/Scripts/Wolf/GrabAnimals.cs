using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAnimals : MonoBehaviour
{
    [HideInInspector] public bool isTouchingSheep;
    [HideInInspector] public bool isTouchingEatable;
    public bool isTouchingFence = false;
    public bool isTouchingDropPoint;
    public Transform mouth;
    public int sheepValue = 1;
    // public float stealthCD = 1f;
    [HideInInspector] public Transform sheep;

    private WolfController wolfController;
    private SheepsClothing sheepsClothing;
    private LevelManager levelManager;
    private GameManager gameManager;
    WolfEmotes we;
    public IEatableAnimal eatableAnimal;
    EventManager em;
    SpaceToRunAway spaceToRunAway;
    public bool isTutBlockingExit = false;
    private SoundManager soundManager;


    void Start()
    {
        wolfController = GetComponent<WolfController>();
        sheepsClothing = GetComponent<SheepsClothing>();
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();
        we = GetComponent<WolfEmotes>();
        em = FindObjectOfType<EventManager>();
        spaceToRunAway = FindObjectOfType<SpaceToRunAway>();
        spaceToRunAway.StopFlicker();
        spaceToRunAway.aintHungry = false;
        soundManager = FindObjectOfType<SoundManager>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gameManager.currentGameState != GameManager.GameState.EndGame)
        {
            if(isTouchingSheep && !wolfController.isCarryingSheep)
            {
                soundManager.PlayBell();
                sheep.SetParent(gameObject.transform, true);
                sheep.GetComponent<CircleCollider2D>().enabled = false;

                wolfController.IsCarryingSheep(true);
                sheepsClothing.Stealth(false);
                we.Emote(1);
                em.GrabbedDatSheep();
                return;
            }
            else if (wolfController.isCarryingSheep && !isTouchingFence)
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
        if (other.gameObject.tag == "Fences")
        {
            isTouchingFence = true;
        }
         
    
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "DropPoint" && !isTutBlockingExit)
        {
            isTouchingDropPoint = true;

            if (wolfController.isCarryingSheep)
            {
                soundManager.PlayBell();
                wolfController.IsCarryingSheep(false);
                levelManager.levelData.aliveAnimals[sheep.name] = false;
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
        if (other.gameObject.tag == "Fences")
        {
            isTouchingFence = false;
        }
     }


        void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.tag == "DropPoint" && !isTutBlockingExit)
            {
                spaceToRunAway.StartFlicker();
            }
            
        }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "DropPoint")
        {
            isTouchingDropPoint = false;
            spaceToRunAway.StopFlicker();
        }

         
    }

    public void EnterForest()
    {
      spaceToRunAway.StartFlicker();
    }

    
}
