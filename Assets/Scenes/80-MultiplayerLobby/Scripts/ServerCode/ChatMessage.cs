using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    /// <summary>
    /// Sets the textOnIngredientHolder for a chat message.
    /// </summary>
    public void SetText(string str)
    {
        messageText.text = str;
    }
}
