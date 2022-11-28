using QuantumTek.QuantumUI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle soundToggle;

    private SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        Load();
    }

    public void ChangeVolume()
    {
        soundManager.ChangeVolume(volumeSlider.value);
        Save();
    }

    public void OnButtonPress()
    {
        if(soundManager.muted)
        {
            soundManager.MuteAudio(false);
        } else
        {
            soundManager.MuteAudio(true);
        }
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SoundManager.soundVolumeKey, volumeSlider.value);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(SoundManager.soundVolumeKey);
        soundToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(SoundManager.soundMutedKey) == 1 ? false : true);
        soundToggle.GetComponent<QUI_SwitchToggle>().SetToggleGraphic();
    }
}
