using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParserTool
{
    
}

public class DialogueManager : SingletonMonoBehavior<DialogueManager>
{
    [SerializeField] List<DialogueSceneData> dialogueSceneData;

    public void StartDialogue(DialogueSystemValidData.DIALOGUE_ID id)
    {

    }
}

[CreateAssetMenu(fileName = "New Data", menuName = "Dialogue/Data")]
public class DialogueSceneData : ScriptableObject
{
    [SerializeField] ConversationData conversationData;
}

[System.Serializable]
public class DialogueData
{
    public bool WickIsSpeaker;
    public string Dialogue;
}

[System.Serializable]
public class ConversationData
{
    public string ID;
    public string Conversant;
    public string Gives;
    public string Unlocks;
    public List<DialogueData> Dialogues;
    public List<DialogueBranchData> Choices;
    public List<DialogueBranchData> LeadsTo;
}

[System.Serializable]
public class DialogueBranchData
{
    public string BranchText;
    public List<string> Requirments;
}

public class DialogueSystemValidData
{
    public readonly static List<string> VALID_CONVERSANTS = new List<string>()
    {
        "Martyn",
        "Belamont",
        "Letter",
        "Newspaper",
        "Bottles",
        "Brochures",
        "Glass"
    };

    public enum DIALOGUE_ID
    {
        Glass1,
        Glass2,
        Glass3,
        Letter1,
        Letter2,
        Letter3
    };
}
