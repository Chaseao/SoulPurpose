using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueSystemValidData.DIALOGUE_ID dialogueID;
    public void ExecuteDialogue()
    {
        print("Starting Letter Dialogue");
        DialogueManager.Instance.StartDialogue(dialogueID);
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }
}
