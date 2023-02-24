using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities;
using UnityEditor;
using static DialogueHelperClass;

public static class JsonDialogueConverter
{
    private static readonly string ID_MARKER = "ID: ";
    private static readonly string CONVERSANT_MARKER = "Conversant: ";
    private static readonly string UNLOCKS_MARKER = "Unlocks: ";
    private static readonly string DIALOGUE_MARKER = "Dialogue:";
    private static readonly string WICK_MARKER = "Wick: ";
    private static readonly string VOICE_MARKER = "Voice: ";
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

            string filePath = $"Assets/Dialogue/{conversation.Data.ID}.asset";
            if (System.IO.File.Exists(filePath))
            {
                var file = AssetDatabase.LoadAssetAtPath(filePath, typeof(SOConversationData)) as SOConversationData;
                file.SetConversation(conversation.Data);
            }
            else
            {
                AssetDatabase.CreateAsset(conversation, filePath);
            }
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

        conversation.ID = lines[0];
        Debug.Log($"Converting {lines[0]}");
        lines.RemoveAt(0);


        AssertMarker(lines[0], CONVERSANT_MARKER);
        conversation.Conversant = lines[0].Substring(CONVERSANT_MARKER.Length);
        lines.RemoveAt(0);

        AssertMarker(lines[0], SOUND_MARKER);
        lines.RemoveAt(0);
        for(int i = 0; i < 6; i++)
        {
            conversation.EmotionsValue[i] = int.Parse(lines[0].Split(": ")[1].Trim());
            lines.RemoveAt(0);
        }

        AssertMarker(lines[0], UNLOCKS_MARKER);
        conversation.Unlocks = lines[0].Substring(UNLOCKS_MARKER.Length);
        lines.RemoveAt(0);

        AssertMarker(lines[0], DIALOGUE_MARKER);
        lines.RemoveAt(0);

        while (!lines[0].StartsWith(CHOICES_MARKER))
        {
            if (!lines[0].StartsWith(WICK_MARKER) && !lines[0].StartsWith($"{conversation.Conversant}: ") && !lines[0].StartsWith(VOICE_MARKER))
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
                else if (lines[0].StartsWith(VOICE_MARKER))
                {
                    dialogueLine = lines[0].Substring(VOICE_MARKER.Length);
                }
                else
                {
                    dialogueLine = lines[0].Substring(conversation.Conversant.Length + ": ".Length);
                }
                conversation.Dialogues.Add(new DialogueData() { Dialogue = dialogueLine.Trim(), VoiceSpeaker = lines[0].StartsWith(VOICE_MARKER), WickIsSpeaker = lines[0].StartsWith(WICK_MARKER) });
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
