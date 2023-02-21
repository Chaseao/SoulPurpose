using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] ButtonGroup menuButtons;
    [SerializeField] AudioControls audioControls;

    private void Start()
    {
        Controller.Instance.SwapToUI();
        menuButtons.EnableButtons();
        audioControls.SetAudio(new int[] { 50 }, false) ;
    }
}
