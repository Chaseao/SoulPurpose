using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass1 : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueSystemValidData.DIALOGUE_ID dialogueID;
    public void ExecuteDialogue()
    {
        print("Starting Glass1 Dialogue");
        DialogueManager.Instance.StartDialogue(dialogueID);
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }
}
