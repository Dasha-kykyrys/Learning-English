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

    // Анимация персонажа
    public Animator anim;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Получение направление передвижения
        movement = new Vector2(joystick.Horizontal, joystick.Vertical);

        // Обновление анимации
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        // Перемещение персонажа
        rigidbody2d.MovePosition(rigidbody2d.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void UpdateAnimation()
    {
        // Определение направления персонажа
        if (movement.magnitude > 0.1f) 
        {
            anim.SetFloat("directionX", movement.x);
            anim.SetFloat("directionY", movement.y);
        }

        // Определение движется ли персонаж
        anim.SetFloat("speed", movement.magnitude);
    }
}