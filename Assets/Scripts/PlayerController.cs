using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Передвижение персонажа
    [SerializeField] private Joystick joystick;
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rigidbody2d;
    private Vector2 movement;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Определение направления
        movement = new Vector2(joystick.Horizontal, joystick.Vertical);

        // Перемещение персонажа
        rigidbody2d.MovePosition(rigidbody2d.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
}