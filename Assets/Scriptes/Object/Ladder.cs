using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
            
            if (Input.GetKey(KeyCode.W))
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
