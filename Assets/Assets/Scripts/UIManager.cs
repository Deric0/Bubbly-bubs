using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ThirdPersonMovement PlayerScript;
    public GameObject UICamera;

    public GameObject ScannedText;
    public GameObject Hologram1;
    public GameObject Hologram2;
    public GameObject InGameCanvas;
    public AudioSource Music;
    public AudioSource Scanning;

    void Start()
    {
        PlayerScript.enabled = false;
        ScannedText.SetActive(false);
        Hologram1.SetActive(false);
        Hologram2.SetActive(false);
        InGameCanvas.SetActive(false);
    }
    public void PlayThatThang()
    {
        PlayerScript.enabled = true;
        UICamera.SetActive(false);
        InGameCanvas.SetActive(true);
        Music.volume = 0.0833f;
    }

    public void Scanny()
    {
        ScannedText.SetActive(true);
        Hologram1.SetActive(true);
        Hologram2.SetActive(true);
        Scanning.Play();
    }


}
