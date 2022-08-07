using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource ambientSource;
    public AudioSource sfxSource;

    public AudioClip currentBGM;

    public AudioClip menuSelectSound;

    public void SetBGM(AudioClip newBGM)
    {
        if(currentBGM != newBGM)
        {
            StartCoroutine(FadeToNewBGM(newBGM));
        }
    }

    private IEnumerator FadeToNewBGM(AudioClip newBGM)
    {
        float duration = 1f;
        float elapsedTime = 0f;
        while(elapsedTime < duration *.5f)
        {
            elapsedTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(1, 0, elapsedTime / (duration * .5f));
            yield return null;
        }
        currentBGM = newBGM;
        bgmSource.clip = currentBGM;
        bgmSource.Play();
        elapsedTime = 0f;
        while (elapsedTime < duration * .5f)
        {
            elapsedTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0, 1, elapsedTime / (duration * .5f));
            yield return null;
        }
    }

    public void PlayMenuSelectSound()
    {
        PlaySoundEffect(menuSelectSound);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
