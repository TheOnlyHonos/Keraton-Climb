using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    [Header("Sensitivity Parameters")] 
    public float Sensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * Sensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //apply to camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * Sensitivity);
    }
}
