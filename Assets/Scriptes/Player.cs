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
        Swim = 4,
    }

    public bool InWater = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Main main;
    [SerializeField] private Transform groundCheck;
    private bool isGround;

    private int curHp;
    private int maxHp = 3;
    private bool isHit = false;

    private bool key;
    private bool canTP = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        curHp = maxHp;
    }

    void FixedUpdate() 
    {    
        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Update()
    {
        CheckGround();

        if (InWater)
        {
            anim.SetInteger("State",(int) State.Swim);
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            anim.SetInteger("State",(int) State.Idle);
        }
    }

    private void Run()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        Flip();

        if (isGround && !InWater)
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.3f);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Key":
                Destroy(other.gameObject);
                key = true;
                break;

            case "Door":
                if (other.gameObject.GetComponent<Door>().isOpen && canTP)
                {
                    other.gameObject.GetComponent<Door>().Teleport(gameObject);
                    canTP = false;
                    StartCoroutine(TPWait());
                }
                else if (key)
                {
                    other.gameObject.GetComponent<Door>().Unlock();
                }
                break;

            default:
                break;
        }   
    }

    private IEnumerator TPWait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Ladder")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}