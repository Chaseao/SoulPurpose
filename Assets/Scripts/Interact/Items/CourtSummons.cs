using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtSummons : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue;
    public void ExecuteDialogue()
    {
        print("Starting CourtSummons Dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }
}
