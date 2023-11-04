using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum State
    {
        Idle = 1,
        Walk = 2,
        Jump = 3,
        Swim = 4,
        ClimbIdle = 5,
        Climbing = 6
    }

    public bool InWater = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float speed;
    [SerializeField] private float speedLadder;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Main main;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    private bool isGround;
    private bool isClimbing = false;

    private int curHp;
    private int maxHp = 3;
    private int coins = 0;
    private bool isHit = false;

    private bool key;
    private bool canTP = true;
    
    public void RecountHp(int deltaHp)
    {
        curHp = (curHp + deltaHp <= maxHp) ? curHp + deltaHp : curHp;

        if (deltaHp < 0)
        {
            StopCoroutine(OnHit());

            isHit = true;

            StartCoroutine(OnHit());
        }

        print("Хп осталось: " + curHp);
        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        curHp = maxHp;
    }

    private void FixedUpdate() 
    {    
        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
    }

    private void Update()
    {
        CheckGround();

        if (InWater && !isClimbing)
        {
            anim.SetInteger("State",(int) State.Swim);
        }

        if (isGround && !isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else if (!isClimbing && !InWater && !isGround)
        {
            anim.SetInteger("State",(int) State.Jump);
        }

        if (Input.GetAxis("Horizontal") == 0 && !isClimbing && !InWater)
        {
            anim.SetInteger("State",(int) State.Idle);
        }
    }

    private void Run()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        Flip();

        if (isGround && !InWater && !isClimbing)
        {
            anim.SetInteger("State",(int) State.Walk);
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
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
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.3f, ground);
    }


    private IEnumerator OnHit()
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

            case "Coin":
                Destroy(other.gameObject);
                coins++;
                print($"Всего монет: {coins}");
                break;

            case "Heart":
                Destroy(other.gameObject);
                RecountHp(1);
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
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0;
            
            if (Input.GetKey(KeyCode.W))
            {
                LadderMov(speedLadder);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                LadderMov(-speedLadder);
            }
            else
            {
                LadderMov(0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1;
        }
    }

    private void LadderMov(float speed)
    {
        rb.velocity = new Vector2(0, speed);

        if (speed == 0)
        {
            anim.SetInteger("State",(int) State.ClimbIdle);
        }
        else
        {
            anim.SetInteger("State",(int) State.Climbing);
        }
    }
}