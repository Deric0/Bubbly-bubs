using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour
{
    public Transform PlayerPosition;
    private Rigidbody RigidbodyOrb;
    public AudioSource OrbBlip;

    private void Start()
    {
        RigidbodyOrb = GetComponent<Rigidbody>();
}
    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerPosition.position) < 5)
        {
            transform.LookAt(PlayerPosition.position);
            RigidbodyOrb.AddForce(transform.forward, ForceMode.VelocityChange);
        }
        if (Vector3.Distance(transform.position, PlayerPosition.position) < 0.44f)
        {
            Destroy(this.gameObject);
            OrbBlip.Play();
        }
    }
}
