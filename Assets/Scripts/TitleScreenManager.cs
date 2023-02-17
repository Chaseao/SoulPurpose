using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] ButtonGroup menuButtons;

    private void Start()
    {
        Controller.Instance.SwapToUI();
        menuButtons.EnableButtons();
    }
}
