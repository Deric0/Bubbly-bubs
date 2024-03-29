using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform Player;
    public Animator PlayerController;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    public float speed = 6f;
    public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 velocity;
    public float gravity = -9.81f;

    public ParticleSystem ShotParticles;
    public GameObject Bubble;
    public Transform BubbleOriginPoint;

    public Rigidbody projectile;

    public GameObject PlayerObject;
    public GameObject Portal1Camera;
    public GameObject Portal2Camera;
    public GameObject Portal3Camera;
    public GameObject BlubPortal1;
    public GameObject BlubDestination1;
    public GameObject BlubPortal2;
    public GameObject BlubDestination2;
    public GameObject BlubPortal3;
    public GameObject BlubDestination3;
    private bool isUsingPortal = false;
    public GameObject Portal1Clone;
    public Animation Portal1CloneAnimation;
    public GameObject Portal2Clone;
    public Animation Portal2CloneAnimation;
    public GameObject Portal3Clone;
    public Animation Portal3CloneAnimation;

    public Cinemachine.CinemachineImpulseSource source;

    public GameObject MainCamera;
    public GameObject AimCamera;
    public GameObject AimReticle;
    private bool isAiming;
    private bool isRunning;
    private bool walkFix;

    public AudioSource BubbleShootingSound;
    public AudioSource BubblePortalSound;
    public AudioSource BubblePortalExitSound;

    public Cinemachine.CinemachineFreeLook FreeLookCamera;

    public Transform Spawnlocation;
    public Transform ThirdPersonFollow;

    public Text Counter;
    public Text CounterShadow;
    private float SecretScore;

    public Text HelpText;
    public Text EndText;

    public Image LeaveButton;
    public Text LeaveButtonText;

    public AudioSource BackgroundMusic;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AimCamera.SetActive(false);
        AimReticle.SetActive(false);
        Portal1Camera.SetActive(false);
        Portal1Clone.SetActive(false);
        Portal2Camera.SetActive(false);
        Portal2Clone.SetActive(false);
        Portal3Camera.SetActive(false);
        Portal3Clone.SetActive(false);
        StartCoroutine(ShowHelpText());
        EndText.color = new Color(1, 1, 1, 0);
        LeaveButtonText.color = new Color(LeaveButtonText.color.r,LeaveButtonText.color.g,LeaveButtonText.color.b,0);
        LeaveButton.color = new Color(LeaveButton.color.r, LeaveButton.color.g, LeaveButton.color.b, 0);
    }
    public IEnumerator ShowHelpText()
    {
        Color Origin = new Color(1,1,1,0);
        Color Destination = new Color(1,1,1,1);
        float totalMovementTime = 5f;
        float currentMovementTime = 0f;
        while (Origin.a != Destination.a)
        {
            currentMovementTime += Time.deltaTime;
            HelpText.color = Color.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }
    public IEnumerator EndThanks()
    {
        yield return new WaitForSeconds(20f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Color Origin = new Color(1, 1, 1, 0);
        Color Destination = new Color(1, 1, 1, 1);
        Color OriginButton = new Color(0, 0, 0, 0);
        Color DestinationButton = new Color(0, 0, 0, 1);
        float totalMovementTime = 5f;
        float currentMovementTime = 0f;
        while (Origin.a != Destination.a)
        {
            currentMovementTime += Time.deltaTime;
            EndText.color = Color.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            LeaveButtonText.color = Color.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            LeaveButton.color = Color.Lerp(OriginButton, DestinationButton, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetMouseButtonDown(1) && AimCamera.activeInHierarchy != true)
        {
            MainCamera.SetActive(false);
            AimCamera.SetActive(true);
            AimReticle.SetActive(true);
            isAiming = true;
            PlayerController.SetTrigger("IsAiming");
            PlayerController.SetBool("StillAiming", true);
            speed -= 4;
        }
        else if (Input.GetMouseButtonUp(1) && MainCamera.activeInHierarchy != true)
        {
            MainCamera.SetActive(true);
            AimCamera.SetActive(false);
            AimReticle.SetActive(false);
            isAiming = false;
            PlayerController.SetBool("StillAiming", false);
            if ((Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) && isAiming == false)
            {
                PlayerController.SetTrigger("Idle");
            }
            else if (isRunning == true)
            {
                PlayerController.SetTrigger("IsRunning");
            }
            else
            {
                PlayerController.SetTrigger("IsWalking");
            }
            speed += 4;
        }

            if (Input.GetMouseButtonDown(0) && isUsingPortal != true)
        {
            HelpText.enabled = false;
            source = GetComponent<Cinemachine.CinemachineImpulseSource>();
            source.GenerateImpulse();
            ShotParticles.Play();
            Rigidbody clone;
            clone = Instantiate(projectile, BubbleOriginPoint.position, transform.rotation);
            clone.tag = "goodBubble";
            clone.velocity = transform.TransformDirection(Vector3.forward * 10);
            PlayerController.SetTrigger("Shooting");
            BubbleShootingSound.pitch = Random.Range(1, 1.3f);
            BubbleShootingSound.Play();
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
       
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
                        speed += 10;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
                        speed -= 10;
        }
        if (((horizontal != 0 || vertical != 0) && isRunning != true) && walkFix != true)
        {
            PlayerController.SetBool("StillWalking", true);
            //Debug.Log("Started Walking");
            if (isAiming != true)
            {
                PlayerController.SetTrigger("IsWalking");
                
            }

            walkFix = true;
        }
        else if ((horizontal == 0 && vertical == 0) && walkFix == true)
        {
            PlayerController.SetBool("StillWalking", false);
            //Debug.Log("Stopped Walking");
            if (isAiming != true)
            {
                
                PlayerController.SetTrigger("Idle");
            }
            walkFix = false;
        }
        if (direction.magnitude >= 0.1 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            //Debug.Log("Started Running");
            if (isAiming != true)
            {
                PlayerController.SetBool("StillWalking", false);
                PlayerController.SetBool("StillRunning", true);
                PlayerController.SetTrigger("IsRunning");
            }
                isRunning = true;
        }
        else if ((direction.magnitude <= 0 && Input.GetKeyDown(KeyCode.LeftShift)) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            //Debug.Log("Stopped Running");
            if ((horizontal == 0 && vertical == 0) && isAiming != true)
            {
                PlayerController.SetTrigger("Idle");
                PlayerController.SetBool("StillRunning", false);
            }
            else if (isAiming != true)
            {
                PlayerController.SetBool("StillWalking", true);
                PlayerController.SetBool("StillRunning", false);
                PlayerController.SetTrigger("IsWalking");
            }

            isRunning = false;
        }
        if (direction.magnitude >= 0.1f && isAiming!=true)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else if (isAiming == true)
        {   
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0,angle,0);

            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * direction;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            }
    }
    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(5);
        Player.transform.position = Spawnlocation.position;
        FreeLookCamera.m_Follow = ThirdPersonFollow;
        BubblePortalExitSound.Play();
    }
    public IEnumerator MoveCamera()
    {
        Vector3 Origin = MainCamera.transform.position;
        Vector3 Destination = MainCamera.transform.position + new Vector3(0, 20, 0);
        float totalMovementTime = 3f;
        float currentMovementTime = 0f;
        while (Vector3.Distance(MainCamera.transform.position, Destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            MainCamera.transform.position = Vector3.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }
    public IEnumerator MoveCamera2()
    {
        Vector3 Origin = MainCamera.transform.position;
        Vector3 Destination = MainCamera.transform.position + new Vector3(0, 100, 0);
        float totalMovementTime = 32f;
        float currentMovementTime = 0f;
        while (Vector3.Distance(MainCamera.transform.position, Destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            MainCamera.transform.position = Vector3.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            yield return null;
        }
        while (BackgroundMusic.volume != 0.9)
        {
            BackgroundMusic.volume += (Time.deltaTime/7);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Orb")
        {
            SecretScore += 1;
            Counter.text = SecretScore.ToString();
            CounterShadow.text = SecretScore.ToString();
        }
        if (other.gameObject.tag == "Finish")
        {
            if (AimCamera.activeInHierarchy == true)
            {
                AimCamera.SetActive(false);
                MainCamera.SetActive(true);
            }
            FreeLookCamera.m_Follow = null;
            FreeLookCamera.m_LookAt = null;
            StartCoroutine(MoveCamera2());
            StartCoroutine(EndThanks());
        }
        if (other.gameObject.tag == "Respawn")
        {
            if(AimCamera.activeInHierarchy == true)
            {
                AimCamera.SetActive(false);
                MainCamera.SetActive(true);
            }
            FreeLookCamera.m_Follow = null;
            StartCoroutine(MoveCamera());
            StartCoroutine(RespawnPlayer());
        }
        if (other.gameObject == BlubPortal1 && isUsingPortal != true)
        {
            //launch 1
                AimCamera.SetActive(false);
                MainCamera.SetActive(false);
            Portal1Camera.SetActive(true);

            PlayerObject.SetActive(false);
            Portal1Clone.SetActive(true);

            Portal1CloneAnimation.Play();
            Player.transform.position = BlubDestination1.transform.position;
            StartCoroutine(DestinationReached1());
            BubblePortalSound.Play();
        }
        else if (other.gameObject == BlubPortal2 && isUsingPortal != true)
        {
            //launch 2
            AimCamera.SetActive(false);
            MainCamera.SetActive(false);
            Portal2Camera.SetActive(true);

            PlayerObject.SetActive(false);
            Portal2Clone.SetActive(true);

            Portal2CloneAnimation.Play();
            Player.transform.position = BlubDestination2.transform.position;
            StartCoroutine(DestinationReached2());
            BubblePortalSound.Play();
        }
        else if (other.gameObject == BlubPortal3 && isUsingPortal != true)
        {
            //launch 3
            AimCamera.SetActive(false);
            MainCamera.SetActive(false);
            Portal3Camera.SetActive(true);
            PlayerObject.SetActive(false);
            Portal3Clone.SetActive(true);

            Portal3CloneAnimation.Play();
            Player.transform.position = BlubDestination3.transform.position;
            StartCoroutine(DestinationReached3());
            BubblePortalSound.Play();
        }

        IEnumerator DestinationReached1()
            {
            isUsingPortal = true;
            yield return new WaitForSeconds(2f);
            isUsingPortal = false;
            Portal1Clone.SetActive(false);
            Portal1Camera.SetActive(false);
            MainCamera.SetActive(true);
            PlayerObject.SetActive(true);
            Player.transform.position = BlubDestination1.transform.position;
            BubblePortalExitSound.Play();
        }
        IEnumerator DestinationReached2()
        {
            isUsingPortal = true;
            yield return new WaitForSeconds(2f);
            isUsingPortal = false;
            Portal2Clone.SetActive(false);
            Portal2Camera.SetActive(false);
            MainCamera.SetActive(true);
            PlayerObject.SetActive(true);
            Player.transform.position = BlubDestination2.transform.position;
            BubblePortalExitSound.Play();
        }
        IEnumerator DestinationReached3()
        {
            isUsingPortal = true;
            yield return new WaitForSeconds(2f);
            isUsingPortal = false;
            Portal3Clone.SetActive(false);
            Portal3Camera.SetActive(false);
            MainCamera.SetActive(true);
            PlayerObject.SetActive(true);
            Player.transform.position = BlubDestination3.transform.position;
            BubblePortalExitSound.Play();
        }
    }
    public void LeaveGame()
    {
        Application.Quit();
    }
}
