using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    [Header("Tutorial components")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] [TextArea] private string[] lines;
    [SerializeField] private float textSpeed;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            switch (index)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (textComponent.text == lines[index])
                        {
                            NextLine();
                        }
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (textComponent.text == lines[index])
                        {
                            NextLine();
                        }
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        if (textComponent.text == lines[index])
                        {
                            NextLine();
                        }
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (textComponent.text == lines[index])
                        {
                            NextLine();
                        }
                    }
                    break;
                case 4:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (textComponent.text == lines[index])
                        {
                            NextLine();
                        }
                    }
                    break;
            }
        }
    }

    void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        //type character 1 by 1
        foreach (char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine(){

        if (index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } 
        else{
            gameObject.SetActive(false);
        }

    }

}
