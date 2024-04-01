using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerMotor : MonoBehaviour
{

    [Header("Player Options")]
    [SerializeField] public bool canMove = true;
    [SerializeField] private bool headBobbing = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool useFootsteps = true;

    [Header("Walk Parameters")]
    [SerializeField] private float defaultSpeed = 3f;
    private float speed;
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float overheadCheckHeight= 1f;
    [SerializeField] private LayerMask overheadObstacle;
    bool overheadCheck;
    private float crouchTimer;
    private bool isCrouching;
    private bool lerpCrouch;

    [Header("Sprint Parameters")]
    [SerializeField] private float sprintSpeed = 5f;
    private bool isSprinting;


    [Header("Jump and Gravity Parameters")]
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    private bool isGrounded;
    [SerializeField] private AudioClip[] jumpStoneClip = default;
    [SerializeField] private AudioClip[] jumpDirtClip = default;
    [SerializeField] private AudioClip[] jumpWoodClip = default;
    [SerializeField] private AudioClip[] jumpRockClip = default;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 10f;
    [SerializeField] private float walkBobAmount = .05f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = .025f;
    [SerializeField] private float sprintBobSpeed = 14f;
    [SerializeField] private float sprintBobAmount = .1f;
    private float defaultYPos = 0;
    private float timer;
    public Camera cam;

    [Header("Footsteps Parameters")]
    [SerializeField] private float baseStepSpeed = 0.6f;
    [SerializeField] private float crouchStepMultiplier = 1.2f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioClip[] stoneClip = default;
    [SerializeField] private AudioClip[] dirtClip = default;
    [SerializeField] private AudioClip[] woodClip = default;
    [SerializeField] private AudioClip[] rockClip = default;
    private float footStepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    [SerializeField] private Transform raycastCheckPoint;
    [SerializeField] private AudioSource playerAudioSource = default;
    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerUI = GetComponent<PlayerUI>();
        speed = defaultSpeed;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = cam.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        overheadCheck = OverheadDetect();

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1f;
            p *= p;

            if (isCrouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    //recieve inputs from InputManager.cs script and apply them to character controller
    public void ProcessMove(Vector2 input)
    {
        if (canMove)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

            playerVelocity.y += gravity * Time.deltaTime;

            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }

            controller.Move(playerVelocity * Time.deltaTime);
            HandleHeadbob(input);
            HandleFootsteps(input);
        }
    }

    public void Jump()
    {
        if (isGrounded && canJump && canMove)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            if (Physics.Raycast(raycastCheckPoint.transform.position, Vector3.down, out RaycastHit hit, 10))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Stone":
                        playerAudioSource.PlayOneShot(jumpStoneClip[Random.Range(0, jumpStoneClip.Length - 1)]);
                        break;
                    case "Footsteps/Wood":
                        playerAudioSource.PlayOneShot(jumpWoodClip[Random.Range(0, jumpWoodClip.Length - 1)]);
                        break;
                    case "Footsteps/Rock":
                        playerAudioSource.PlayOneShot(jumpRockClip[Random.Range(0, jumpRockClip.Length - 1)]);
                        break;
                    default:
                        playerAudioSource.PlayOneShot(jumpDirtClip[Random.Range(0, jumpDirtClip.Length - 1)]);
                        break;
                }
            }
        }
    }

    public void Crouch()
    {
        if (!overheadCheck)
        {
            if (canCrouch)
            {
                isSprinting = false;
                isCrouching = !isCrouching;
                if (isCrouching)
                {
                    speed = crouchSpeed;
                    canSprint = false;
                }
                else
                {
                    speed = defaultSpeed;
                    canSprint = true;
                }
                crouchTimer = 0;
                lerpCrouch = true;
            }
        }
        else playerUI.ShowCantStandHere();
    }

    public void Sprint()
    {
        if (canSprint)
        {
            isSprinting = !isSprinting;
            if (isSprinting) speed = sprintSpeed;
            else speed = defaultSpeed;
        }
    }

    public void HandleHeadbob(Vector2 input)
    {
        if (headBobbing && canMove)
        {
            if (!isGrounded) return;

            if (Mathf.Abs(input.x) > .1f || Mathf.Abs(input.y) > .1f)
            {
                timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);

                cam.transform.localPosition = new Vector3(
                    cam.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
                    cam.transform.localPosition.z);

            }   else if (cam.transform.localPosition.y != defaultYPos)
            {
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, defaultYPos, cam.transform.localPosition.z);
            }
        }
    }

    public void HandleFootsteps(Vector2 input)
    {
        if (useFootsteps)
        {
            if (!isGrounded) return;
            if (input == Vector2.zero) return;

            footStepTimer -= Time.deltaTime;
            if (footStepTimer < 0)
            {
                if (Physics.Raycast(raycastCheckPoint.transform.position, Vector3.down, out RaycastHit hit, 3))
                {
                    switch (hit.collider.tag)
                    {
                        case "Footsteps/Stone":
                            playerAudioSource.PlayOneShot(stoneClip[Random.Range(0, stoneClip.Length - 1)]);
                            break;
                        case "Footsteps/Wood":
                            playerAudioSource.PlayOneShot(woodClip[Random.Range(0, woodClip.Length - 1)]);
                            break;
                        case "Footsteps/Rock":
                            playerAudioSource.PlayOneShot(rockClip[Random.Range(0, rockClip.Length - 1)]);
                            break;
                        default:
                            playerAudioSource.PlayOneShot(dirtClip[Random.Range(0, dirtClip.Length - 1)]);
                            break;
                    }
                }
                footStepTimer = GetCurrentOffset;
            }
        }
    }

    bool OverheadDetect()
    {
        Ray ray = new Ray(raycastCheckPoint.position, raycastCheckPoint.up);
        Debug.DrawRay(ray.origin, ray.direction * overheadCheckHeight);
        RaycastHit hitInfo; //variable to store our collision info
        if (Physics.Raycast(ray, out hitInfo, overheadCheckHeight, overheadObstacle))
        {
            return true;
        }
        return false;
    }
}
