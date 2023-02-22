using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martyn : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue1;
    [SerializeField] private SOConversationData dialogue2;
    [SerializeField] private InteractSystem interact;
    public static int hasStartedMartynDialogue;
    void Start()
    {
        hasStartedMartynDialogue = 0;
    }

    public bool ExecuteDialogue()
    {
        if(hasStartedMartynDialogue== 0)
        {
            DialogueManager.Instance.StartDialogue(dialogue1);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(dialogue2);
        }
        hasStartedMartynDialogue++;
        return true;
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }

    public void EndLevel()
    {
        if (SceneTools.NextSceneExists)
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
        if (DialogueManager.Instance.dialogueUnlocks.Contains("seenend") && !DialogueManager.Instance.InDialogue)
        {
            EndLevel();
        }
    }
}
