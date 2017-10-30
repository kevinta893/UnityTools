using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamTexturer : MonoBehaviour {


    [Header("Initialization")]
    [Tooltip("Lets you choose the camera you wish to use initially.")]
    public int startDeviceIndex = 0;

    [Header("Debug (read only)")]
    public string cameraName;
    public Vector2 resolution;
    public string[] avaliableDevices;


    private WebCamTexture webcamTexture;

    void Start() {
        WebCamDevice[] camDevices = WebCamTexture.devices;

        if (startDeviceIndex >= camDevices.Length)
        {
            Debug.LogError("Device index out of bounds, no such camera exists! Device count=" + camDevices.Length);
            return;
        }

        //create the new webcam texture and start it
        WebCamTexture webcamTexture = new WebCamTexture();
        webcamTexture.deviceName = camDevices[startDeviceIndex].name;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();


        //display debug information
        this.cameraName = webcamTexture.deviceName;
        this.resolution = new Vector2(webcamTexture.width, webcamTexture.height);
        this.avaliableDevices = new string[camDevices.Length];
        for(int i = 0; i < camDevices.Length; i++)
        {
            this.avaliableDevices[i] = camDevices[i].name;
        }

        Debug.Log("Using camera= " + webcamTexture.deviceName + " (" + camDevices.Length + " total devices)");
    }



    public void Play()
    {
        webcamTexture.Play();
    }

    public void Pause()
    {
        webcamTexture.Pause();
    }


    //Returns status of webcam texture's playing. True of playing, false if paused
    public bool TogglePausePlay()
    {
        if (webcamTexture.isPlaying)
        {
            //is playing, now pause
            webcamTexture.Pause();
            return false;
        }
        else
        {
            //is playing, now pause
            webcamTexture.Play();
            return true;
        }
    }
}
