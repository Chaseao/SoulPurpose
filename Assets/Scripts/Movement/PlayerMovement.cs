using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{

    private new Rigidbody rigidbody;
    private Vector3 movementForce;
    private Vector2 playerInput;
    private float maxX;
    private float maxZ;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float speed = 0.25f;

    private void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        maxX = maxSpeed;
        maxZ = maxSpeed;
    }
    private void Update()
    {
        //Check for whether or not player is going higher than the max speed, added in another check for if the player wants to change directions
        if (Math.Abs(rigidbody.velocity.x) > maxX && (Mathf.Sign(movementForce.x) == Mathf.Sign(rigidbody.velocity.x))) movementForce.x = 0;
        if (Math.Abs(rigidbody.velocity.z) > maxZ && (Mathf.Sign(movementForce.z) == Mathf.Sign(rigidbody.velocity.z))) movementForce.z = 0;
        this.rigidbody.AddForce(movementForce, ForceMode.VelocityChange);
        //Allows player to not slide
        if(playerInput.Equals(Vector2.zero))
        {
            rigidbody.velocity = Vector3.zero;
        }

    }

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
        if(!canMove)
        {
            input = Vector3.zero;
        }
        playerInput= input;
        input *= speed;
        movementForce = new Vector3(input.x, 0f, input.y);

    }
}
