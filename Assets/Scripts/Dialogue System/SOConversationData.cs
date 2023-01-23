using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Dialogue/Data")]
public class SOConversationData : ScriptableObject
{
    [SerializeField] ConversationData conversationData;

    public ConversationData Data => conversationData;
}
