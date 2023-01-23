using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private void OnEnable()
    {
        Controller.OnMove += Move;
    }

    private void OnDisable()
    {
        Controller.OnMove -= Move;
    }

    void Move(Vector2 input)
    {
        
    }
}
