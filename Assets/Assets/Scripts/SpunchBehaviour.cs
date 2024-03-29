using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpunchBehaviour : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform enemyTransform;
    public Transform Player;
    public float detectionRange = 15f;
    public float jumpCooldown = 50f;
    private float jumpCooldownTime = 0f;
    private float jumpTime = 0;

    public Rigidbody EnemyRigidbody;
    public float jumpForce = 5;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    public GameObject Bubble;
    private float BubbledTime = 7;
    public LayerMask Projectile;
    private bool isBubbled = false;

    public Animator SpunchController;

    private bool SpunchAlerted = false;
    private float AlertedCooldown;

    private AudioSource BubbledSound;
    public AudioSource BubbledBurstSound;

    private float AttackCooldown;
    private bool Attacking;

    public AudioSource SpunchBubbledGroan;
    public AudioSource SpunchTired;
    private float SpunchTiredCooldown;
    public AudioSource SpunchAlertedSound;

    private void Start()
    {
        BubbledSound = GetComponent<AudioSource>();
        AlertedCooldown = jumpCooldown;
        Bubble.SetActive(false);
        jumpCooldownTime = 0;
        enemy.enabled = false;
        SpunchController.SetTrigger("Idle");
        SpunchTiredCooldown = Random.Range(4, 10);
    }

    void Bubbled()
    {
        BubbledSound.Play();
        SpunchBubbledGroan.Play();
        isBubbled = true;
        Bubble.SetActive(true);
        BubbledTime = 7;
        SpunchController.SetTrigger("IsBubbled");
        if(SpunchAlerted != true)
        {
            SpunchAlerted = true;
            AlertedCooldown = 0;
        }
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
        SpunchTiredCooldown -= Time.deltaTime;
        if (SpunchTiredCooldown <= 0 && SpunchAlerted != true)
        {
            SpunchTiredCooldown = Random.Range(3, 9);
            SpunchTired.Play();
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (BubbledTime <= 0 && isBubbled == true)
        {
            isBubbled = false;
            enemy.enabled = true;
            Bubble.SetActive(false);
            BubbledBurstSound.Play();
        }
        if(isBubbled == true)
        {
            BubbledTime -= Time.deltaTime;
            enemy.enabled = false;
        }
        if (SpunchAlerted == true)
        {
            AlertedCooldown -= Time.deltaTime;
        }
        if ((Vector3.Distance(enemyTransform.position, Player.position) < 3f) && SpunchAlerted == true && AlertedCooldown <= 0 && isGrounded==true && isBubbled != true && AttackCooldown <= 0)
        {
            AttackCooldown = 2;
            Attacking = true;
            SpunchController.SetTrigger("IsAttacking");
            Debug.Log("Attack");
        }
        if(Attacking == true)
        {
            AttackCooldown -= Time.deltaTime;
            if(AttackCooldown <= 0)
            {
                Attacking = false;
            }
            if (AttackCooldown <= 1)
            {
                SpunchController.SetTrigger("IsWalking");
            }
        }
            if ((Vector3.Distance(enemyTransform.position, Player.position) < detectionRange) && SpunchAlerted == true && AlertedCooldown <= 0)
        {
            JumpToPlayer();
        }
        if ((Vector3.Distance(enemyTransform.position, Player.position) < detectionRange) && SpunchAlerted != true)
        {
            SpunchAlerted = true;
            SpunchAlertedSound.Play();
            SpunchController.SetTrigger("IsAlerted");
            enemy.enabled = false;
            StartCoroutine(PrepareAttack());
        }
            if (isGrounded == true)
        {
            jumpCooldownTime -= Time.deltaTime;
        }
            else if (isGrounded == false && isBubbled == false)
        {
            jumpTime += Time.deltaTime;
        }
      
        if (enemy.enabled == true)
        {
            enemy.SetDestination(Player.position);
        }
    }

    private void  JumpToPlayer()
    {        
        if (isGrounded == true && jumpCooldownTime <= 0 && isBubbled != true)
        {
            StartCoroutine(Jumpy());
            SpunchController.SetTrigger("IsJumping");
        }
    }
    IEnumerator PrepareAttack()
    {
        yield return new WaitForSeconds(2.3f);
        JumpToPlayer();
    }
    IEnumerator Jumpy()
    {
        yield return new WaitForSeconds(0.6f);
        jumpTime = 0;
        StopChasingPlayer();
        jumpCooldownTime = jumpCooldown;
        enemyTransform.LookAt(new Vector3(Player.position.x, enemyTransform.position.y, Player.position.z));
        EnemyRigidbody.velocity = new Vector3(enemyTransform.forward.x / 3, enemyTransform.up.y, enemyTransform.forward.z / 3) * jumpForce;
        StartCoroutine(LandAfterJump());
    }
    IEnumerator LandAfterJump()
    {
        yield return new WaitForSeconds(4.20f);
        ChasingPlayer();
    }

    private void StopChasingPlayer()
    {
        enemy.enabled = false;
    }

    private void ChasingPlayer()
    {
        SpunchController.SetTrigger("IsWalking");
        enemy.enabled = true;
    }

}
