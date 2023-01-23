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
        if (Math.Abs(rigidbody.velocity.x) > maxX) movementForce.x = 0;
        if (Math.Abs(rigidbody.velocity.z) > maxZ) movementForce.z = 0;
        this.rigidbody.AddForce(movementForce, ForceMode.VelocityChange);
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
        playerInput= input;
        input *= speed;
        movementForce = new Vector3(input.x, 0f, input.y);

    }
}
