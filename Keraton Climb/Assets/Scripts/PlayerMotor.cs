using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMotor : MonoBehaviour
{

    [Header("Player Options")]
    [SerializeField] private bool headBobbing = true;
    [SerializeField] private bool canJump = true;

    [Header("Walk Parameters")]
    [SerializeField] private float speed = 5f;
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Jump and Gravity Parameters")]
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    private bool isGrounded;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 12f;
    [SerializeField] private float walkBobAmount = .05f;
    private float defaultYPos = 0;
    private float timer;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
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
    }

    //recieve inputs from InputManager.cs script and apply them to character controller
    public void ProcessMove(Vector2 input)
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

    public void Jump()
    {
        if (isGrounded && canJump)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void HandleHeadbob(Vector2 input)
    {
        if (headBobbing)
        {
            if (!isGrounded) return;

            if (Mathf.Abs(input.x) > .1f || Mathf.Abs(input.y) > .1f)
            {
                timer += Time.deltaTime * (walkBobSpeed);
                cam.transform.localPosition = new Vector3(
                    cam.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(timer) * walkBobAmount,
                    cam.transform.localPosition.z);
            }   else if (cam.transform.localPosition.y != defaultYPos)
            {
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, defaultYPos, cam.transform.localPosition.z);
            }
        }
    }
}
