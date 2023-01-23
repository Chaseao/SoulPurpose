using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParserTool
{
    
}

public class DialogueData
{
    public bool WickIsSpeaker;
    public string Dialogue;
}

public class ConversationData
{
    public string Conversant;
    public List<DialogueData> Dialogues;
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
}
