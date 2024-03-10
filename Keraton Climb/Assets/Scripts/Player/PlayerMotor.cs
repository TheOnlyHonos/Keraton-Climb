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
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool headBobbing = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canSprint = true;

    [Header("Walk Parameters")]
    [SerializeField] private float defaultSpeed = 3f;
    private float speed;
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float overheadCheckHeight= 1f;
    [SerializeField] private Transform overheadCheckPoint;
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
        }
    }

    public void Jump()
    {
        if (isGrounded && canJump)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
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
        else CantStandHereTMP();
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

    bool OverheadDetect()
    {
        Ray ray = new Ray(overheadCheckPoint.position, overheadCheckPoint.up);
        Debug.DrawRay(ray.origin, ray.direction * overheadCheckHeight);
        RaycastHit hitInfo; //variable to store our collision info
        if (Physics.Raycast(ray, out hitInfo, overheadCheckHeight, overheadObstacle))
        {
            return true;
        }
        return false;
    }

    public void CantStandHereTMP()
    {
        playerUI.ShowCantStandHere();
    }
}
