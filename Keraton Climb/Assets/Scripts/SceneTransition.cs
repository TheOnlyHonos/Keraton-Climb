using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private TextMeshProUGUI transitionTMP;
    [SerializeField] private string currentLevelName;
    [SerializeField] private string nextLevelName;
    public float transitionTime = 1f;

    private void Start()
    {
        transitionTMP.text = currentLevelName;
    }

    public void StartTransition()
    {
        transitionTMP.text = nextLevelName;
        transition.SetTrigger("Start");
    }
}
