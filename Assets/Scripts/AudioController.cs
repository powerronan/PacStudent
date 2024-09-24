using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource introAudio;
    public AudioSource backgroundAudio;

    void Start()
    {
        introAudio.Play();

        backgroundAudio.PlayScheduled(AudioSettings.dspTime + introAudio.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
