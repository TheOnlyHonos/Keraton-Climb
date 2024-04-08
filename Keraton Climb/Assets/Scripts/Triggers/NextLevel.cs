using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [Header("Player Object Parameters")]
    [SerializeField] private GameObject player;

    private PlayerHealthAndHunger healthAndHunger;
    private SceneTransition transition;
    private float transitionTime;

    private void Start()
    {
        healthAndHunger = player.GetComponent<PlayerHealthAndHunger>();
        transition = player.GetComponent<SceneTransition>();
        transitionTime = transition.transitionTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthAndHunger.SaveParameterForNextLevel();

            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            /*if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }*/

            StartCoroutine(SceneTransition(nextSceneIndex));
        }
    }

    IEnumerator SceneTransition(int sceneIndex)
    {
        transition.StartTransition();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
