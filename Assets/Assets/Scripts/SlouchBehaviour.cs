using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlouchBehaviour : MonoBehaviour
{
    public GameObject slouchCollider;
    public BoxCollider OwnCollider;
    public AudioSource SlouchSound1;
    public AudioSource SlouchSound2;
    public AudioSource SlouchSound3;
    private float SlouchSoundInterval;
    private float SlouchSoundPicker;

    public GameObject Bubble;
    private float BubbledTime;
    public LayerMask Projectile;
    private bool isBubbled = false;

    public Animator SlouchController;

    private AudioSource BubbledSound;
    public AudioSource BubbledBurstSound;

    private void Start()
    {
        BubbledSound = GetComponent<AudioSource>();
        Bubble.SetActive(false);
        SlouchController.SetTrigger("Idle");
    }
    void Bubbled()
    {
        BubbledSound.Play();
        isBubbled = true;
        Bubble.SetActive(true);
        slouchCollider.SetActive(false);
        OwnCollider.enabled = false;
        BubbledTime = 5;
        SlouchController.SetTrigger("Bubbled");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "goodBubble")
        {
            Destroy(other.gameObject);
            Bubbled();
        }
    }
    void Update()
    {
        if (BubbledTime <= 0 && isBubbled == true)
        {
            isBubbled = false;
            Bubble.SetActive(false);
            slouchCollider.SetActive(true);
            OwnCollider.enabled = true;
            SlouchController.SetTrigger("Idle");
            BubbledBurstSound.Play();
        }
        if (isBubbled == true)
        {
            BubbledTime -= Time.deltaTime;
        }

        SlouchSoundInterval -= Time.deltaTime;

        if (SlouchSoundInterval <= 0)
        {
            SlouchSoundInterval = Random.Range(5, 15);
            SlouchSound();
        }

    }

    void SlouchSound()
    {
        SlouchSoundPicker = Random.Range(1, 3);
        switch (SlouchSoundPicker)
        {
            case 1:
                SlouchSound1.Play();
                    break;
            case 2:
                SlouchSound2.Play();
                break;
            case 3:
                SlouchSound3.Play();
                break;
        }
    }
}
