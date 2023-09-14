using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManageeer : MonoBehaviour
{
    public static AudioManageeer instance; // Instância única do AudioManager

    public AudioSource soundEffectSource; // AudioSource para os efeitos sonoros
    public AudioSource musicSource; // AudioSource para a música de fundo

    public AudioClip[] audiosPlacar;

    void Awake()
    {
        // Configurar a instância única do AudioManager
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

    // Método para reproduzir um efeito sonoro
    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }

    // Método para reproduzir música de fundo
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Método para pausar a música de fundo
    public void PauseMusic()
    {
        musicSource.Pause();
    }

    // Método para retomar a reprodução da música de fundo
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}
