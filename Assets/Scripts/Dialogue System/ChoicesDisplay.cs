using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoicesDisplay : MonoBehaviour
{
    [SerializeField] GameObject choiceTemplate;

    List<TextMeshProUGUI> choices = new List<TextMeshProUGUI>();

    public void Display(List<string> validChoices)
    {
        foreach(var choiceOption in validChoices)
        {
            GameObject instance = Instantiate(choiceTemplate, transform);
            var textBox = instance.transform.GetComponentInChildren<TextMeshProUGUI>();
            textBox.text = choiceOption;
            textBox.color = Color.gray;
            choices.Add(textBox);
        }

        if (choices.Count > 0) SelectChoice(0);
    }

    public void SelectChoice(int index)
    {
        choices.ForEach(choice => choice.color = Color.gray);
        choices[index].color = Color.black;
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
