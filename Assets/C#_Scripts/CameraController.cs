using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;        //Public variable to store a reference to the player game object
    private Vector3 offset;            //Private variable to store the offset distance between the player and camera
    //private Camera cameraComponet;
    private float cameraZoomStart = 6;  //預設：5
    private int courseClearChangeTimes = 0;
    private int courseClearAllTimes = 400;
    private Vector3 cameraMoveDestination = new Vector3(186.03f, 8.31f, -10f);
    private float   cameraZoomDestination = 13;
    private Vector3 cameraMoveSpeed;
    private float   cameraZoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        this.transform.position = new Vector3(-9.92f, 1.24f, -10f);
        offset = transform.position - player.transform.position;
        //cameraComponet = gameObject.GetComponent<Camera>();
        Camera.main.orthographicSize = cameraZoomStart;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        if(Player.courseClear)
        {
            if(courseClearChangeTimes == 0)
            {
                cameraMoveSpeed = (cameraMoveDestination - this.transform.position) / courseClearAllTimes;
                cameraZoomSpeed = (cameraZoomDestination - cameraZoomStart) / courseClearAllTimes;
                courseClearChangeTimes++;
            }
            else if(courseClearChangeTimes < courseClearAllTimes)
            {
                this.transform.position = this.transform.position + cameraMoveSpeed;
                Camera.main.orthographicSize += cameraZoomSpeed;
                courseClearChangeTimes++;
                //Debug.Log(courseClearChangeTimes);
            }
            else if(courseClearChangeTimes == courseClearAllTimes)
            {
                this.transform.position = cameraMoveDestination;
                Camera.main.orthographicSize = cameraZoomDestination;
                courseClearChangeTimes++;
            }
        }
        else
        {
            transform.position = player.transform.position + offset;
        }
    }
}
