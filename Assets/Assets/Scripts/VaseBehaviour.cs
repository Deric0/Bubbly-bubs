using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseBehaviour : MonoBehaviour
{

    private Animator VaseAnimator;
    private float RandomBreakSound;

    public AudioSource BreakSound1;
    public AudioSource BreakSound2;
    public AudioSource BreakSound3;

    private readonly float Volume = 0.3f;

    private ParticleSystem DustParticle;

    private bool Broken = false;

    public Rigidbody OrbCollectible;

    private void Start()
    {
        VaseAnimator = GetComponent<Animator>();
        DustParticle = GetComponent<ParticleSystem>();
        BreakSound1.volume = Volume;
        BreakSound2.volume = Volume;
        BreakSound3.volume = Volume;
    }
    void Orbs()
    {
        Rigidbody Orb;
        Orb = Instantiate(OrbCollectible, transform.position, transform.rotation);
        Orb.AddForce(Vector3.up * 7, ForceMode.Impulse);
        Orb.AddForce(Vector3.right * Random.Range(-1, 1), ForceMode.Impulse);
        Orb.AddForce(Vector3.forward * Random.Range(-1, 1), ForceMode.Impulse);

        Rigidbody Orb1;
        Orb1 = Instantiate(OrbCollectible, transform.position, transform.rotation);
        Orb1.AddForce(Vector3.up * 7, ForceMode.Impulse);
        Orb1.AddForce(Vector3.right * Random.Range(-1, 1), ForceMode.Impulse);
        Orb1.AddForce(Vector3.forward * Random.Range(-1, 1), ForceMode.Impulse);

        Rigidbody Orb2;
        Orb2 = Instantiate(OrbCollectible, transform.position, transform.rotation);
        Orb2.AddForce(Vector3.up * 7, ForceMode.Impulse);
        Orb2.AddForce(Vector3.right * Random.Range(-1, 1), ForceMode.Impulse);
        Orb2.AddForce(Vector3.forward * Random.Range(-1, 1), ForceMode.Impulse);

    }
    void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.tag == "goodBubble")
        {
            RandomBreakSound = Random.Range(1, 3);
            if (Broken == false)
            {
                DustParticle.Play();
                VaseAnimator.SetTrigger("Break");
                PlaySound();
                Orbs();
            }
            Broken = true;
        }
    }
    void PlaySound()
    {
        switch (RandomBreakSound)
        {
            case 1:
                BreakSound1.pitch = Random.Range(0.8f, 1.3f);
                BreakSound1.Play();
                break;
            case 2:
                BreakSound2.pitch = Random.Range(0.8f, 1.3f);
                BreakSound2.Play();
                break;
            case 3:
                BreakSound3.pitch = Random.Range(0.8f, 1.3f);
                BreakSound3.Play();
                break;
        }
    }
    void Update()
    {
    }
}
