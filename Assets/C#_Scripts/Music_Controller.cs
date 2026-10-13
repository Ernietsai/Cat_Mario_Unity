using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music_Controller : MonoBehaviour
{
    AudioSource audioSource;
    private bool playedMusic = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(SceneManager.GetActiveScene().name);// == "Cat_Mario_Scene"
        if(SceneManager.GetActiveScene().name == "Cat_Mario_Scene")
        {   if(!(playedMusic))
            {
                audioSource.Play();
                playedMusic = true;
            }
        }
        else
        {
            audioSource.Stop();
        }
        
    }
}
