using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

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

        if(index == 0){
            if(Input.GetKeyDown(KeyCode.Return)){
                if(textComponent.text == lines[index]){
                    NextLine();
                }
            }
        }

        if(index == 1){
            if(Input.GetKeyDown(KeyCode.Space)){
                if(textComponent.text == lines[index]){
                    NextLine();
                }
            }
        }

         if(index == 2){
            if(Input.GetKeyDown(KeyCode.LeftShift)){
                if(textComponent.text == lines[index]){
                    NextLine();
                }
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
