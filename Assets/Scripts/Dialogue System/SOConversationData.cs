using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Dialogue/Data")]
public class SOConversationData : ScriptableObject
{
    [SerializeField] ConversationData conversationData;
    [SerializeField] TextAsset jsonConversationData;

    public ConversationData Data => jsonConversationData != null ? JsonDialogueConverter.ConvertFromJson(jsonConversationData) : conversationData;
}
