using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Father : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue;
    private bool hasStartedFatherDialogue;


    void Start()
    {
        hasStartedFatherDialogue = false;
    }

    public bool ExecuteDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        hasStartedFatherDialogue = true;
        return true;
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }

    public void EndLevel()
    {
        if(SceneTools.NextSceneExists)
        {
            StartCoroutine(SceneTools.TransitionToScene(SceneTools.NextSceneIndex));
        }
        else
        {
            Application.Quit();
        }
    }

    void Update()
    {
        if(hasStartedFatherDialogue && !DialogueManager.Instance.InDialogue)
        {
            EndLevel();
        }
    }
}
