using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundCheck;
    private bool isCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate() 
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

        if (isCollider && Input.GetButton("Jump"))
        {
            Jump();
        }
    }

    void Update()
    {
        Flip();
        CheckGround();
    }

    private void Jump() => rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);

    private void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            sr.flipX = false;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            sr.flipX = true;
        }
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isCollider = colliders.Length > 1;
    }
}
