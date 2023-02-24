using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] ButtonGroup buttons;

    private void Start()
    {
        DisableMenu();
        Controller.OnPause += PauseGame;
    }

    public void ResumeGame()
    {
        Controller.Instance.SwapToGameplay();
        DisableMenu();
        Controller.OnResume -= ResumeGame;
    }

    private void DisableMenu()
    {
        canvas.enabled = false;
        buttons.DisableButtons();
    }

    public void PauseGame()
    {
        Controller.OnResume += ResumeGame;
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