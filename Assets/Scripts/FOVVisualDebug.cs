using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVisualDebug : MonoBehaviour
{
    [SerializeField] GameObject fovDog;
    [SerializeField] GameObject fovFarmer;
    [SerializeField] GameObject fovDog2;
    public bool fovIsOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (fovIsOn)
            {
                fovDog.SetActive(false);
                if (fovDog2 != null)
                fovDog2.SetActive(false);
                fovFarmer.SetActive(false);
                fovIsOn = false;
            }

            else if (!fovIsOn)
            {
                fovDog.SetActive(true);
                if (fovDog2 != null)
                fovDog2.SetActive(true);
                fovFarmer.SetActive(true);
                fovIsOn = true;
            }
        }
    }
}
