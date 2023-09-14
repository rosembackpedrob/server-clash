using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManageeer : MonoBehaviour
{
    public static AudioManageeer instance; // Inst�ncia �nica do AudioManager

    public AudioSource soundEffectSource; // AudioSource para os efeitos sonoros
    public AudioSource musicSource; // AudioSource para a m�sica de fundo

    public AudioClip[] audiosPlacar;

    void Awake()
    {
        // Configurar a inst�ncia �nica do AudioManager
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // M�todo para reproduzir um efeito sonoro
    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }

    // M�todo para reproduzir m�sica de fundo
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.loop = true;
    }

    // M�todo para pausar a m�sica de fundo
    public void PauseMusic()
    {
        musicSource.Pause();
    }

    // M�todo para retomar a reprodu��o da m�sica de fundo
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}
