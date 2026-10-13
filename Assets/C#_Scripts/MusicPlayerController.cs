using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerController : MonoBehaviour
{
    //public static bool courseClear = false;
    public bool playingMusic = true;
    private AudioSource audioSourse;
    // Start is called before the first frame update
    void Start()
    {
        audioSourse = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.courseClear)
        {
            if(playingMusic)
            {
                audioSourse.Stop();
                playingMusic = false;
            }
        }
    }
}
