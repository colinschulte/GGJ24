using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] AudioSource m_Source;
    [SerializeField] AudioSource sfx_Source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void playMusic(AudioClip music)
    {
        m_Source.clip = music;
        m_Source.Play();
    }

    public void playSFX(AudioClip soundEffect)
    {
        sfx_Source.PlayOneShot(soundEffect);
    }
}
