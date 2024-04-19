using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveWaypoint : MonoBehaviour
{

    [SerializeField] private Image objectiveMarker;
    [SerializeField] private Transform objectiveLocation;
    [SerializeField] private TextMeshProUGUI distanceTMP;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        float minX = objectiveMarker.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = objectiveMarker.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(objectiveLocation.position + offset);

        if(Vector3.Dot((objectiveLocation.position - transform.position), transform.forward) < 0)
        {
            if(pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        objectiveMarker.transform.position = pos;
        distanceTMP.text = Vector3.Distance(objectiveLocation.position, transform.position).ToString("0") + "m"; ;
    }
}
