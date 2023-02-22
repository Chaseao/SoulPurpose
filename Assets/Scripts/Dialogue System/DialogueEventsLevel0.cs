using System.Collections;
using UnityEngine;
using static DialogueHelperClass;

public class DialogueEventsLevel0 : MonoBehaviour
{
    [SerializeField] GameObject imageToDisplay;
    [SerializeField] SOConversationData imageConversation;
    [SerializeField] AudioSource audioToPlayFrom;
    [SerializeField] SOConversationData audioConversation;

    bool triggerImage;
    bool triggerAudio;

    private void OnEnable()
    {
        DialogueManager.OnDialogueStarted += UpdateLastDialogue;
        //DialogueManager.OnDialogueEnded += CheckForEvent;
        DialogueManager.OnChoiceMenuClose += CheckForEvent;
    }

    private void UpdateLastDialogue(ConversationData obj)
    {
        triggerImage = imageConversation.Data.Equals(obj);
        triggerAudio = audioConversation.Data.Equals(obj);
    }

    private void CheckForEvent()
    {
        if (triggerImage)
        {
            StartCoroutine(PlayImage());
        }
        if (triggerAudio)
        {
            audioToPlayFrom.enabled = false;
        }
    }

    private IEnumerator PlayImage()
    {
        imageToDisplay.SetActive(true);
        yield return new WaitForSeconds(5);
        imageToDisplay.SetActive(false);
    }


    private void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= UpdateLastDialogue;
        DialogueManager.OnChoiceMenuClose -= CheckForEvent;
    }
}