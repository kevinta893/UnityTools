using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamTexturer : MonoBehaviour {


    [Header("Initialization")]
    [Tooltip("Lets you choose the camera you wish to use initially.")]
    public int startDeviceIndex = 0;
    [Tooltip("Dumps out debug information into the log. Useful for Android/iOS device debugging")]
    public bool dumpDebugInformation = false;


    [Header("Debug (read only)")]
    public string activeCameraName;
    public Vector2 resolution;
    public string[] avaliableDevices;
    public float aspectRatio = 0.0f;

    private WebCamTexture webcamTexture;

    void Start() {
        WebCamDevice[] camDevices = WebCamTexture.devices;

        if (startDeviceIndex >= camDevices.Length)
        {
            Debug.LogError("Device index out of bounds, no such camera exists! Device count=" + camDevices.Length);
            return;
        }

        //create the new webcam texture and start it
        webcamTexture = new WebCamTexture();
        webcamTexture.deviceName = camDevices[startDeviceIndex].name;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();


        //display debug information
        this.activeCameraName = webcamTexture.deviceName;
        this.resolution = new Vector2(webcamTexture.width, webcamTexture.height);
        this.aspectRatio = (float) webcamTexture.width / (float) webcamTexture.height;
        this.avaliableDevices = new string[camDevices.Length];
        for(int i = 0; i < camDevices.Length; i++)
        {
            this.avaliableDevices[i] = camDevices[i].name;
        }


        Debug.Log("Webcam started, using camera= " + webcamTexture.deviceName + " (" + camDevices.Length + " total devices)");
        
        if (dumpDebugInformation)
        {
            Debug.Log(
                "Camera name=" + this.activeCameraName + "\n" +
                "Resolution=" + this.resolution.ToString() + "\n" +
                "Aspect Ratio=" + this.aspectRatio + "\n" +
                "Total Devices=" + this.avaliableDevices.Length + "\n"
                );
        }
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
