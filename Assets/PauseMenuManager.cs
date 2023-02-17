using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] ButtonGroup buttons;

    private void Start()
    {
        DisableMenu();
        Controller.OnPause += PauseGame;
        Controller.OnResume += ResumeGame;
    }

    public void ResumeGame()
    {
        if (DialogueManager.Instance.InDialogue) return;
        Controller.Instance.SwapToGameplay();
        DisableMenu();
    }

    private void DisableMenu()
    {
        canvas.enabled = false;
        buttons.DisableButtons();
    }

    public void PauseGame()
    {
        if (DialogueManager.Instance.InDialogue) return;

        Controller.Instance.SwapToUI();
        canvas.enabled = true;
        buttons.EnableButtons();
    }

    private void OnDestroy()
    {
        buttons.DisableButtons();
        Controller.OnPause -= PauseGame;
        Controller.OnResume -= ResumeGame;
    }
}