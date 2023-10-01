using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate() 
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    void Update()
    {
        
    }
}
