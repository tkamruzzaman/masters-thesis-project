using DialogueEditor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Test : MonoBehaviour
{
    [FormerlySerializedAs("Conversation")] 
    public NPCConversation conversation;

    [Button]
    [ContextMenu("StartConversation")]
    private void StartConversation()
    {
        ConversationManager.Instance.StartConversation(conversation);
    }
}
