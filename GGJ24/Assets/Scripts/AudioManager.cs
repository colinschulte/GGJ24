using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_Source;
    [SerializeField] AudioSource sfx_Source;

    public void playMusic(AudioClip music)
    {
        m_Source.Stop();
        m_Source.clip = music;
        m_Source.Play();
    }

    public void playSFX(AudioClip soundEffect)
    {
        sfx_Source.PlayOneShot(soundEffect);
    }
}
