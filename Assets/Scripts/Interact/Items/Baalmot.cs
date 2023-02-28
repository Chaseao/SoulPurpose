using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baalmot : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue;

    [SerializeField] GameObject portal;



    public bool ExecuteDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        return true;
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }

    public void StartPortal()
    {
        portal.SetActive(true);
    }

    void Update()
    {
        if(DialogueManager.Instance.dialogueUnlocks.Contains("givesoul") && !DialogueManager.Instance.InDialogue)
        {
            StartPortal();
        }
    }
}
