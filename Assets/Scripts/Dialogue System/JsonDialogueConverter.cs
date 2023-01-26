using System.Collections.Generic;
using UnityEngine;

public static class JsonDialogueConverter
{
    public static void ConvertToJson(ConversationData conversation)
    {
        string jsonFile = JsonUtility.ToJson(conversation, true);
        string conversationID = conversation.ID;
        System.IO.File.WriteAllText(Application.dataPath + $"/Dialogue/{conversationID}.json", jsonFile);
    }
}

[System.Serializable]
public class DialogueData
{
    public bool WickIsSpeaker;
    [SerializeField, TextArea()]public string Dialogue;
}

[System.Serializable]
public class ConversationData
{
    public string ID;
    public string Conversant;
    public string Unlocks;
    public List<DialogueData> Dialogues;
    public Dictionary<DialogueSystemValidData.SoundEmotions, int> Emotions;
    public List<DialogueBranchData> Choices;
    public List<DialogueBranchData> LeadsTo;
}

[System.Serializable]
public class DialogueBranchData
{
    public string BranchText;
    public List<string> Requirements;
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
        glass1,
        glass2,
        glass3,
        letter1,
        letter2,
        letter3
    };

    public enum SoundEmotions
    {
        sorrow,
        pain,
        warmth,
        fundamental,
        bliss,
        fear
    }
}
