using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBubbleFix : MonoBehaviour
{
    public Transform SoundPosition;
    public AudioSource PopSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "goodBubble")
        {
            SoundPosition.position = other.gameObject.transform.position;
            PopSound.pitch = Random.Range(0.7f,1);
            PopSound.Play();
            Destroy(other.gameObject);
        }
    }
    float Raycast;






    float lookingTime;
    bool startAnimations;

    float totalLookTime = 10;

    private void Update()
    {
        if(Raycast == Raycast) //The repeating raycast Statement
        {
            lookingTime += Time.deltaTime;
        }
        if(lookingTime >= totalLookTime)
        {
            AnimationEvent();    
        }
    }
    void AnimationEvent()
    {
        if (startAnimations != true)
        {
            startAnimations = true;
            //Play Animations
        }
    }



}
