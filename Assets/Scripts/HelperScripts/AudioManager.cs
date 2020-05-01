using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip PickUpSound;
    public AudioClip DeadSound;
    public AudioSource audioSource; 
    
    
    // Start is called before the first frame update
    void Awake()
    {
        CreateInstace();
        audioSource = GetComponent<AudioSource>();
    }

    void CreateInstace()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayPickUpSound()
    {
        audioSource.PlayOneShot(PickUpSound);
    }
    
    
    public void PlayDeadSound()
    {
        audioSource.PlayOneShot(DeadSound);
    }
}
