using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gopher : MonoBehaviour, IEatableAnimal
{
    private WolfController wolfController;
    private LevelManager levelManager;
    [SerializeField] GameObject bloodObj;
    SheepsClothing sheepsClothing;
    public GameObject gopherFlickerObj;
    private SoundManager soundManager;

    private void Awake() {
        EventManager.OnGopherStartFlicker += GopherFlicker;
    }
    void Start()
    {
        wolfController = FindObjectOfType<WolfController>();
        sheepsClothing = wolfController.GetComponent<SheepsClothing>();
        levelManager = FindObjectOfType<LevelManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IGotEaten()
    {
        soundManager.PlayChew();
        levelManager.GopherEaten();
        sheepsClothing.Stealth(false);
        wolfController.GetComponent<WolfEmotes>().Emote(0); // call emotion "anim"
        var blood = Instantiate(bloodObj, transform.position, Quaternion.identity);
        blood.GetComponent<BloodStain>().BloodSplatter();
        levelManager.levelData.aliveAnimals[gameObject.name] = false;
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

    public void GopherFlicker()
    {
        if (gopherFlickerObj!= null)
     StartCoroutine(GopherFlickerRepeat());
    }

    IEnumerator GopherFlickerRepeat()
    {
        if (gopherFlickerObj != null)
        {
        while (true)
        {
        if (gopherFlickerObj.activeInHierarchy)
        gopherFlickerObj.SetActive(false);
        else 
        gopherFlickerObj.SetActive(true);
      yield return new WaitForSeconds(0.3f);
      }
      }
    }
}
