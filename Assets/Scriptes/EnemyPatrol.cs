using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyPatrol : Enemy
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private bool moveLeft = true;
    [SerializeField] private Transform groundDetect;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, 1f);

        if (groundInfo.collider == false)
        {
            if (moveLeft == true)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                moveLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveLeft = true;
            }
        }
    }
}