using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMartynSystem : MonoBehaviour
{
    [SerializeField] private DialogueSystemValidData.DIALOGUE_ID dialogueID;

    private void OnEnable()
    {
        Controller.OnMartynInteract += InteractMartyn;
    }

    private void OnDisable()
    {
        Controller.OnMartynInteract -= InteractMartyn;
    }

    void InteractMartyn()
    {
        print("Starting Martyn Dialogue");
        DialogueManager.Instance.StartDialogue(dialogueID);
    }
}
