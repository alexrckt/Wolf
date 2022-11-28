using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip shoot;
    public AudioClip bell;
    public AudioClip chew;
    public AudioClip bulletImpact;
    public List<AudioClip> sniffs;
    private bool sniffing = false;
    public List<AudioClip> barks;
    private bool barking = false;
    public AudioClip dogBite;
    public AudioClip backgroundMusic;
    public bool muted;

    public static string soundVolumeKey = "soundVolume";
    public static string soundMutedKey = "soundMuted";

    void Start()
    {
        if (!PlayerPrefs.HasKey(soundVolumeKey))
            PlayerPrefs.SetFloat(soundVolumeKey, 1f);

        if (!PlayerPrefs.HasKey(soundMutedKey))
        {
            PlayerPrefs.SetInt(soundMutedKey, 0);
            muted = false;
        } else
        {
            muted = PlayerPrefs.GetInt(soundMutedKey) == 1 ? true : false;
        }

        MuteAudio(muted);
        ChangeVolume(PlayerPrefs.GetFloat(soundVolumeKey));
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void MuteAudio(bool muted)
    {
        AudioListener.pause = muted;
        this.muted = muted;
        PlayerPrefs.SetInt(soundMutedKey, muted ? 1 : 0);
    }

    #region Play methods
    public void PlaySniffing()
    {
        if (!sniffing)
        {
            sniffing = true;
            StartCoroutine("PlaySniffSound");
        }
    }

    public void PlayDogBite()
    {
        PlaySoundOnce(dogBite);
    }

    public void PlayBark()
    {
        if(!barking)
        {
            barking = true;
            StartCoroutine("PlayBarkSound");
        }
    }

    public void PlayShoot()
    {
        PlaySoundOnce(shoot);
    }

    public void PlayBell()
    {
        PlaySoundOnce(bell);
    }

    public void PlayChew()
    {
        PlaySoundOnce(chew);
    }

    public void PlayBulletImpact()
    {
        PlaySoundOnce(bulletImpact);
    }
    
    public void PlayBackgroundMusic()
    {
        if (GameObject.Find("Background Music") != null)
            return;

        GameObject soundGameObject = new GameObject("Background Music");
        soundGameObject.transform.SetParent(GetComponent<GameManager>().transform);
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }


    private void PlaySoundOnce(AudioClip audioClip)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
    }

    private IEnumerator PlayBarkSound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        soundGameObject.transform.SetParent(GetComponent<GameManager>().transform);
        audioSource.PlayOneShot(barks[Random.Range(0, barks.Count)]);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        barking = false;
        Destroy(soundGameObject);
    }

    private IEnumerator PlaySniffSound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        soundGameObject.transform.SetParent(GetComponent<GameManager>().transform);
        audioSource.PlayOneShot(sniffs[Random.Range(0, sniffs.Count)]);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        sniffing = false;
        Destroy(soundGameObject);
    }
    #endregion
}
