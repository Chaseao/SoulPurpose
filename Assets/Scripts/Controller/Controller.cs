using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : SingletonMonoBehavior<Controller>
{
    public static Action<Vector2> OnMove;
    public static Action OnInteract;
    public static Action OnMartynInteract;
    public static Action OnPause;

    public static Action<Vector2> OnNavigateMenu;
    public static Action OnSelect;
    public static Action OnGoBack;
    public static Action OnSkip;
    public static Action OnResume;

    private bool inGameplay = false;
    public bool InGameplay => inGameplay;

    [SerializeField] PlayerInput playerInput;

    public void SwapToUI() { playerInput.SwitchCurrentActionMap("UI"); inGameplay = false; }
    public void SwapToGameplay() { playerInput.SwitchCurrentActionMap("Gameplay"); inGameplay = true; }

    #region Gameplay Layout

    public void Move(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteract?.Invoke();
        }
    }

    public void Pause(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            OnPause?.Invoke();
        }
    }

    public void MartynInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnMartynInteract?.Invoke();
        }
    }

    #endregion

    #region UI Layout

    public void Resume(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            OnResume?.Invoke();
        }
    }

    public void NavigateMenu(InputAction.CallbackContext context)
    {
        if(context.started)
        OnNavigateMenu?.Invoke(context.ReadValue<Vector2>());
    }

    public void GoBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnGoBack?.Invoke();
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnSelect?.Invoke();
        }
    }

    public void Skip(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnSkip?.Invoke();
        }
    }

    #endregion
}
