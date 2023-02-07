using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Father : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue;
    public bool ExecuteDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        EndLevel();
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
}
