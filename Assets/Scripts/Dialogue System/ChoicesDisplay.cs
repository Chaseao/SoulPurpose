using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesDisplay : MonoBehaviour
{
    [SerializeField] GameObject choiceTemplate;
    [SerializeField] float scaleFactor;

    List<TextMeshProUGUI> choicesText = new List<TextMeshProUGUI>();
    List<RectTransform> choices = new List<RectTransform>();


    public void Display(List<string> validChoices)
    {
        foreach(var choiceOption in validChoices)
        {
            GameObject instance = Instantiate(choiceTemplate, transform);
            var textBox = instance.transform.GetComponentInChildren<TextMeshProUGUI>();
            textBox.text = choiceOption;
            textBox.color = Color.gray;
            choicesText.Add(textBox);
            choices.Add(instance.GetComponent<RectTransform>());
        }

        if (choicesText.Count > 0) SelectChoice(0);
    }

    public void SelectChoice(int index)
    {
        choicesText.ForEach(choice => choice.color = Color.gray);
        choicesText[index].color = Color.black;
        if (choices.Count > 0)
        {
            choices.ForEach(x => x.localScale = Vector3.one);
            choices[index].localScale = Vector3.one * scaleFactor;
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
        choicesText.Clear();
        choices.Clear();
    }
}
