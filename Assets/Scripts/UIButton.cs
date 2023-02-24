using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] ButtonFunctions function;
    [SerializeField] int sceneIndex;
    [SerializeField] PauseMenuManager pauseMenuManager;

    enum ButtonFunctions
    {
        transtionToMenu,
        quitGame,
        resumeGame
    }

    public virtual void ToggleSelected(bool isSelected)
    {
        transform.localScale = Vector3.one * (isSelected ? 1.2f : 1);
    }

    public void Use()
    {
        switch (function)
        {
            case ButtonFunctions.transtionToMenu:
                Controller.Instance.SwapToGameplay();
                StartCoroutine(SceneTools.TransitionToScene(sceneIndex));
                break;
            case ButtonFunctions.quitGame:
                Application.Quit();
                break;
            case ButtonFunctions.resumeGame:
                pauseMenuManager.ResumeGame();
                break;
        }
    }
}
