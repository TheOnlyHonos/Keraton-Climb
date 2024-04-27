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
    [SerializeField] private PlayableDirector timeline_Tebing;
    [SerializeField] private GameObject cutsceneCam_Tebing;

    [Header("End Game Trigger Parameters")]
    [SerializeField] private GameObject towerEndGameTrigger;
    [SerializeField] private GameObject soundtrackEndTrigger;
    [SerializeField] private bool isTowerEndGameTrigger = false;
    [SerializeField] private float towerCinematicTime;
    [SerializeField] private float tebingCinematicTime;
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
                var rotationVector = player.transform.rotation.eulerAngles;
                rotationVector.y = 60;
                player.transform.rotation = Quaternion.Euler(0,60,0);

                cutsceneCam_Tebing.SetActive(true);
                playerCam.SetActive(false);
                player.GetComponent<MeshRenderer>().enabled = false;

                playerMotor.canMove = false;
                playerLook.canLook = false;
                playerHunger.enableHunger = false; 
                playerUI.enabled = false;
                
                StartCoroutine(TebingCinematic());
                
                
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

     IEnumerator TebingCinematic()
    {
        timeline_Tebing.Play();
        yield return new WaitForSeconds(tebingCinematicTime);
        cutsceneCam_Tebing.SetActive(false);
        playerCam.SetActive(true);

        player.GetComponent<MeshRenderer>().enabled = false;

        playerMotor.canMove = true;
        playerLook.canLook = true;
        playerHunger.enableHunger = true;
        playerUI.enabled = true;

        objectiveWaypointLvl5.moveObjectiveToTower = true;
        towerEndGameTrigger.SetActive(true);
        soundtrackEndTrigger.SetActive(true);
    }

    IEnumerator SceneTransition(int sceneIndex)
    {
        transition.StartTransition();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
