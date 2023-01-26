using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoicesDisplay : MonoBehaviour
{
    [SerializeField] GameObject choiceTemplate;

    List<TextMeshProUGUI> choices = new List<TextMeshProUGUI>();

    public void Display(ConversationData conversationData)
    {
        foreach(var choiceOption in conversationData.Choices)
        {
            GameObject instance = Instantiate(choiceTemplate, transform);
            var textBox = instance.transform.GetComponentInChildren<TextMeshProUGUI>();
            textBox.text = choiceOption.BranchText;
            textBox.color = Color.gray;
            choices.Add(textBox);
        }
    }

    public void Hide()
    {
        DestroyChildren();
    }

    private void DestroyChildren()
    {
        int children = transform.childCount - 1;
        while(children >= 0)
        {
            Destroy(transform.GetChild(children).gameObject);
            children--;
        }
        choices.Clear();
    }
}
