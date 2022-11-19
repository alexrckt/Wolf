using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gopher : MonoBehaviour, IEatableAnimal
{
    private WolfController wolfController;
    private LevelManager levelManager;
    [SerializeField] GameObject bloodObj;
    SheepsClothing sheepsClothing;
    public float stealthCD = 1f;
    // Start is called before the first frame update
    void Start()
    {
        wolfController = FindObjectOfType<WolfController>();
        sheepsClothing = wolfController.GetComponent<SheepsClothing>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IGotEaten()
    {
        levelManager.GopherEaten();
        sheepsClothing.Stealth(false, stealthCD);
        wolfController.GetComponent<WolfEmotes>().Emote(0); // call emotion "anim"
        var blood = Instantiate(bloodObj, transform.position, Quaternion.identity);
        blood.GetComponent<BloodStain>().BloodSplatter();
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
