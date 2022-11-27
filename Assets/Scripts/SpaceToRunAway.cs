using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceToRunAway : MonoBehaviour
{
    TextMeshProUGUI text;
    private static Material m_TextBaseMaterial;
        private static Material m_TextHighlightMaterial;
        public bool isIncreasing = false;
        float f;
        public float incrSpeed = 0.3f;
        bool aintHungry = false;
        public bool isGoingUp = true;
    // Start is called before the first frame update

    private void Awake() {
        EventManager.OnHungerFull += AbleToFlicker;
    }
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        m_TextBaseMaterial =  text.fontSharedMaterial;
        m_TextHighlightMaterial = new Material(m_TextBaseMaterial);
         m_TextHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isIncreasing)
        {
            if (isGoingUp)
            { 
                f = m_TextHighlightMaterial.GetFloat(ShaderUtilities.ID_GlowPower);
                m_TextHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, f+incrSpeed * Time.deltaTime);
                text.fontSharedMaterial = m_TextHighlightMaterial;
                text.UpdateMeshPadding();
            }
            
            else
            {
               f = m_TextHighlightMaterial.GetFloat(ShaderUtilities.ID_GlowPower);
                m_TextHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, f-incrSpeed * Time.deltaTime);
                text.fontSharedMaterial = m_TextHighlightMaterial;
                text.UpdateMeshPadding(); 
            }
        
         }

         if (text.fontSharedMaterial.GetFloat(ShaderUtilities.ID_GlowPower) >= 0.5f)
         {
           isGoingUp = false;
         }
         if (text.fontSharedMaterial.GetFloat(ShaderUtilities.ID_GlowPower) <= 0)
         {
            isGoingUp = true;
         }
        //text.fontSharedMaterial
    }

    public void StopFlicker()
    {
        isIncreasing = false;
        m_TextHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0);
                text.fontSharedMaterial = m_TextHighlightMaterial;
               text.text = ""; 

    }

    public void StartFlicker()
    {
      if (aintHungry)
        {isIncreasing = true;
        text.text = "Press <Space> to run away"; }
        
    }

    public void AbleToFlicker()
    {
       aintHungry = true;
    }
}
