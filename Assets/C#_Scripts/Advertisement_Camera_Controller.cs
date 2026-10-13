using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Advertisement_Camera_Controller : MonoBehaviour
{
    private float startTime;
    private UnityEngine.Video.VideoPlayer videoplayer;

    // Start is called before the first frame update
    void Start()
    {
        if(!(SceneManager.GetActiveScene().name == "Ernietsai_Advertisement_Player"))
        {
            SceneManager.LoadScene(1);
        }
        startTime = Time.time;
        videoplayer = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
        //Debug.Log(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(0); //此行為跳過廣告用
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time - startTime);
        if(Time.time - startTime > 13)
        {
            SceneManager.LoadScene(0);
        }
    }
}
