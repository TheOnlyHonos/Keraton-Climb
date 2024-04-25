using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class EndGameTrigger : MonoBehaviour
{
    [Header("Player Object Parameters")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private Canvas playerUI;
    private SceneTransition transition;
    private float transitionTime;

    [Header("Cutscene Parameters")]
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject cutsceneCam;

    [Header("End Game Trigger Parameters")]
    [SerializeField] private GameObject towerEndGameTrigger;
    [SerializeField] private bool isTowerEndGameTrigger = false;
    [SerializeField] private float towerCinematicTime;
    private ObjectiveWaypointLvl5 objectiveWaypointLvl5;

    private PlayerHealthAndHunger playerHunger;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;

    // Start is called before the first frame update
    void Start()
    {
        objectiveWaypointLvl5 = Camera.main.GetComponent<ObjectiveWaypointLvl5>();
        playerHunger = player.GetComponent<PlayerHealthAndHunger>();
        playerMotor = player.GetComponent<PlayerMotor>();
        playerLook = player.GetComponent<PlayerLook>();
        transition = player.GetComponent<SceneTransition>();
        transitionTime = transition.transitionTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            if (!isTowerEndGameTrigger)
            {
                objectiveWaypointLvl5.moveObjectiveToTower = true;
                towerEndGameTrigger.SetActive(true);
            }
            else
            {
                cutsceneCam.SetActive(true);
                playerCam.SetActive(false);
                player.GetComponent<MeshRenderer>().enabled = false;

                objectiveWaypointLvl5.objectiveMarker.enabled = false;

                playerMotor.canMove = false;
                playerLook.canLook = false;
                playerHunger.enableHunger = false;
                playerUI.enabled = false;

                StartCoroutine(TowerCinematic());
            }
        }
    }

    IEnumerator TowerCinematic()
    {
        timeline.Play();
        yield return new WaitForSeconds(towerCinematicTime);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(SceneTransition(nextSceneIndex));
    }

    IEnumerator SceneTransition(int sceneIndex)
    {
        transition.StartTransition();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
