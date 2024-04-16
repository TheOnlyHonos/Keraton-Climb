using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.LowLevel;

public class FogDensityTrigger : MonoBehaviour
{
    [Header("Fog Density Transition Parameters")]
    [SerializeField] private float targetDensity = 0.12f;
    [SerializeField] private float transitionDuration = 2f;
    //check isStartTrigger kalau trigger bakal naikin fog, uncheck kalau trigger bakal nurunin fog 
    [SerializeField] private bool isStartTrigger = true;
    private bool hasPassed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isStartTrigger)
            {
                if (!hasPassed)
                {
                    StartCoroutine(FogTransition(0.03f, targetDensity));
                    //RenderSettings.fogDensity = targetDensity;
                    hasPassed = true;
                }
                else
                {
                    StartCoroutine(FogTransition(targetDensity, 0.01f));
                    //RenderSettings.fogDensity = 0.01f;
                    hasPassed = false;
                }
            } else
            {
                if (!hasPassed)
                {
                    StartCoroutine(FogTransition(targetDensity, 0.01f));
                    hasPassed = true;
                }
                else
                {
                    StartCoroutine(FogTransition(0.01f, targetDensity));
                    hasPassed = false;
                }
            }
        }
    }

    IEnumerator FogTransition(float lerpStartDensity, float lerpTargetDensity)
    {
        float timePassed = 0f;

        while (timePassed <= transitionDuration)
        {
            float lerpVactor = timePassed / transitionDuration;
            RenderSettings.fogDensity = Mathf.Lerp(lerpStartDensity, lerpTargetDensity, lerpVactor);
            timePassed += Time.deltaTime;

            yield return null;
        }
    }
}
