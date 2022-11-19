using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gopher : MonoBehaviour, IEatableAnimal
{
    private WolfController wolfController;
    private LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        wolfController = FindObjectOfType<WolfController>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IGotEaten()
    {
        levelManager.GopherEaten();
        wolfController.GetComponent<WolfEmotes>().Emote(0); // call emotion "anim"
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        (this as IEatableAnimal).OnTriggerStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        (this as IEatableAnimal).OnTriggerExit2D(other);
    }
}
