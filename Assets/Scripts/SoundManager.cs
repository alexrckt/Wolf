using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject soundGameObject = new GameObject("Sound");
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
}
