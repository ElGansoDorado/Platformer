using UnityEngine;

public class PlayerMove : MonoBehaviour
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
    

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;

    public bool InWater = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Player player;

    private bool isGround;
    private bool isClimbing = false;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        player = GetComponent<Player>();
    }

    private void Update()
    {
        CheckGround();

        if (InWater && !isClimbing)
        {
            anim.SetInteger("State",(int) State.Swim);
        }

        if (Input.GetAxis("Horizontal") == 0 && !isClimbing && !InWater)
        {
            anim.SetInteger("State",(int) State.Idle);
        }
    }


    public void Run()
    {
        if (Input.GetButton("Horizontal"))
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * player.Speed, rb.velocity.y);
            Flip();

            if (isGround && !InWater && !isClimbing)
            {
                anim.SetInteger("State",(int) State.Walk);
            }
        }
    }

    public void Jump()
    {
        if (isGround && !isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * player.JumpHeight, ForceMode2D.Impulse);
            }
        }
        else if (!isClimbing && !InWater && !isGround)
        {
            anim.SetInteger("State",(int) State.Jump);
        }
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


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0;
            
            if (Input.GetKey(KeyCode.W))
            {
                LadderMov(player.SpeedLadder);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                LadderMov(-player.SpeedLadder);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Quicksand")
        {
            player.Speed *= 0.25f;
            rb.mass *= 100f;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Quicksand")
        {
            player.Speed /= 0.25f;
            rb.mass /= 100f;
        }
    }
}