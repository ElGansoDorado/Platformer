using System;
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
    

    [SerializeField] private float speed;
    [SerializeField] private float speedLadder;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Main main;

    public int CurHp 
    {
        get => curHp; 
        private set
        {
            curHp = value;
            OnHeartInfoEvent?.Invoke();
        }
    }

    public float Speed
    {
        get => speed; 
        set => speed = value;
    }
    public float SpeedLadder
    {
        get => speedLadder; 
        set => speedLadder = value;
    }
    public float JumpHeight
    {
        get => jumpHeight; 
        set => jumpHeight = value;
    }
    public bool CanHit = true;

    private SpriteRenderer sr;
    private PlayerMove move;

    private int curHp;
    private int maxHp = 3;

    private bool isHit = false;


    public event Action OnHeartInfoEvent;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        CurHp = maxHp;

        move = GetComponent<PlayerMove>();
    }

    private void FixedUpdate() 
    {    
        move.Run();
    }

    private void Update()
    {
        move.Jump();
    }


    public void RecountHp(int deltaHp)
    {
        CurHp = ((CurHp + deltaHp <= maxHp) && CanHit) ? CurHp + deltaHp : CurHp;

        if (deltaHp < 0 && CanHit)
        {
            StopCoroutine(OnHit());

            CanHit = false;
            isHit = true;

            StartCoroutine(OnHit());
        }

        if (CurHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    public void Lose()
    {
        main.GetComponent<Main>().Lose();
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

        if (sr.color.g >= 1f)
        {
            StopCoroutine(OnHit());
            CanHit = true;
        }

        if (sr.color.g <= 0)
        {
            isHit = false;
        }

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }
}