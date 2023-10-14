using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.Tilemaps;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum State
    {
        Idle = 1,
        Walk = 2,
        Jump = 3,
    }

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Main main;
    [SerializeField] private Transform groundCheck;
    private bool isGround;

    private int curHp = 3;
    private int maxHp = 3;
    private bool isHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() 
    {
        if (Input.GetAxis("Horizontal") == 0)
        {
            anim.SetInteger("State",(int) State.Idle);
        }
        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
        if (isGround && Input.GetButton("Jump"))
        {
            Jump();
        }
    }

    void Update()
    {
        Flip();
        CheckGround();
    }

    private void Run()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

        if (isGround)
        {
            anim.SetInteger("State",(int) State.Walk);
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);

        anim.SetInteger("State",(int) State.Jump);
    }

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
        isGround = colliders.Length > 1;
    }

    public void RecountHp(int deltaHp)
    {
        curHp = curHp + deltaHp;

        if (deltaHp < 0)
        {
            StopCoroutine(OnHit());

            isHit = true;

            StartCoroutine(OnHit());
        }

        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    IEnumerator OnHit()
    {
        if (isHit)
        {
            sr.color = new Color(1f, sr.color.g - 0.04f, sr.color.b - 0.04f);
        }
        else
        {
            sr.color = new Color(1f, sr.color.g + 0.04f, sr.color.b + 0.04f);
        }

        if (sr.color.g == 1f)
        {
            StopCoroutine(OnHit());
        }

        if (sr.color.g <= 0)
        {
            isHit = false;
        }

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    private void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
}