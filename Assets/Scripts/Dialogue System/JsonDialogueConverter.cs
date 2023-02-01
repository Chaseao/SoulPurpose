using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities;
using UnityEditor;

public static class JsonDialogueConverter
{
    private static readonly string ID_MARKER = "ID: ";
    private static readonly string CONVERSANT_MARKER = "Conversant: ";
    private static readonly string UNLOCKS_MARKER = "Unlocks: ";
    private static readonly string DIALOGUE_MARKER = "Dialogue:";
    private static readonly string WICK_MARKER = "Wick: ";
    private static readonly string SOUND_MARKER = "Sound Profile:";
    private static readonly string CHOICES_MARKER = "Choices:";
    private static readonly string LEADS_TO_MARKER = "Leads to:";

    public static string ConvertToJson(ConversationData conversation)
    {
        string jsonFile = JsonUtility.ToJson(conversation, true);
        string conversationID = conversation.ID;
        //System.IO.File.WriteAllText(Application.dataPath + $"/Dialogue/{conversationID}.json", jsonFile);
        return jsonFile;
    }

    public static void ConvertToJson(string text)
    {
        foreach (string dialogueScene in text.Split(ID_MARKER, StringSplitOptions.RemoveEmptyEntries)) {
            Debug.Log(dialogueScene);
            SOConversationData conversation = ScriptableObject.CreateInstance<SOConversationData>();
            conversation.SetConversation(ConvertFromJson(ConvertToJson(ConvertToConversation(dialogueScene))));
            AssetDatabase.CreateAsset(conversation, $"Assets/Dialogue/{conversation.Data.ID}.asset");
        }
    }

    public static ConversationData ConvertFromJson(string jsonFile)
    {
        return JsonUtility.FromJson<ConversationData>(jsonFile);
    }

    public static ConversationData ConvertFromJson(TextAsset jsonFile)
    {
        return ConvertFromJson(jsonFile.text);
    }

    private static ConversationData ConvertToConversation(string text)
    {
        var conversation = new ConversationData();
        var lines = text.Split('\n').Where(x => !x.IsNullOrWhitespace()).Select(x => x.Trim()).ToList();
        Debug.Log(lines.Count);
        Debug.Log(text.Split('\n').Count());

        //AssertMarker(lines[0], ID_MARKER);
        conversation.ID = lines[0];//.Substring(ID_MARKER.Length);
        lines.RemoveAt(0);

        AssertMarker(lines[0], CONVERSANT_MARKER);
        conversation.Conversant = lines[0].Substring(CONVERSANT_MARKER.Length);
        lines.RemoveAt(0);

        AssertMarker(lines[0], SOUND_MARKER);
        lines.RemoveAt(0);
        for(int i = 0; i < 6; i++)
        {
            //Debug.Assert(DialogueSystemValidData.SOUND_EMOTIONS.Contains(lines[0].Split(": ")[0]), $"INVALID EMOTION AT {lines[0]}");
            conversation.EmotionsValue[i] = 0; //int.Parse(lines[0].Split(": ")[1].Trim());
            lines.RemoveAt(0);
        }

        AssertMarker(lines[0], UNLOCKS_MARKER);
        conversation.Unlocks = lines[0].Substring(UNLOCKS_MARKER.Length);
        lines.RemoveAt(0);

        AssertMarker(lines[0], DIALOGUE_MARKER);
        lines.RemoveAt(0);

        while (!lines[0].StartsWith(CHOICES_MARKER))
        {
            //Debug.Assert(lines[0].StartsWith(WICK_MARKER) || lines[0].StartsWith($"{conversation.Conversant}: "), $"INVALID DIALOGUE STARTER AT LINE {lines[0]}");
            if (!lines[0].StartsWith(WICK_MARKER) && !lines[0].StartsWith($"{conversation.Conversant}: "))
            {
                conversation.Dialogues[conversation.Dialogues.Count - 1].Dialogue += " " + lines[0];
            }
            else
            {
                string dialogueLine = "";
                if (lines[0].StartsWith(WICK_MARKER))
                {
                    dialogueLine = lines[0].Substring(WICK_MARKER.Length);
                }
                else
                {
                    dialogueLine = lines[0].Substring(conversation.Conversant.Length + ": ".Length);
                }
                conversation.Dialogues.Add(new DialogueData() { Dialogue = dialogueLine.Trim(), WickIsSpeaker = lines[0].StartsWith(WICK_MARKER) });
            }

            lines.RemoveAt(0);
        }

        lines.RemoveAt(0);

        while (!lines[0].StartsWith(LEADS_TO_MARKER))
        {
            var choiceOption = lines[0].Split('~').Select(x => x.Trim()).ToArray();
            var branchData = new DialogueBranchData();

            branchData.BranchText = choiceOption[0];
            branchData.Requirements = new List<string>();

            for(int i = 1; i < choiceOption.Length; i++)
            {
                branchData.Requirements.Add(choiceOption[i]);
            }

            conversation.Choices.Add(branchData);
            lines.RemoveAt(0);
        }

        lines.RemoveAt(0);

        while (lines.Count > 0)
        {
            var branchLines = lines[0].Split('~').Select(x => x.Trim()).ToArray();
            var branchData = new DialogueBranchData();

            branchData.BranchText = branchLines[0];
            branchData.Requirements = new List<string>();

            for (int i = 1; i < branchLines.Length; i++)
            {
                branchData.Requirements.Add(branchLines[i]);
            }

            conversation.LeadsTo.Add(branchData);
            lines.RemoveAt(0);
        }

        return conversation;
    }

    private static bool AssertMarker(string text, string marker)
    {
        Debug.Assert(text.StartsWith(marker), $"ERROR: {text} did not start with {marker}");
        return text.StartsWith(marker);
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
    public List<DialogueData> Dialogues = new List<DialogueData>();
    public int[] EmotionsValue = new int[6];
    public List<DialogueBranchData> Choices = new List<DialogueBranchData>();
    public List<DialogueBranchData> LeadsTo = new List<DialogueBranchData>();
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

    public readonly static List<string> SOUND_EMOTIONS = new List<string>()
    {
        "Sorrow",
        "Pain",
        "Warmth",
        "Fundamental",
        "Bliss",
        "Fear"
    };
}
