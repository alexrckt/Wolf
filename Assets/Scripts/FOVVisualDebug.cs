using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVisualDebug : MonoBehaviour
{
    [SerializeField] GameObject fovDog;
    [SerializeField] GameObject fovFarmer;
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
                fovFarmer.SetActive(false);
                fovIsOn = false;
            }

            else if (!fovIsOn)
            {
                fovDog.SetActive(true);
                fovFarmer.SetActive(true);
                fovIsOn = true;
            }
        }
    }
}
