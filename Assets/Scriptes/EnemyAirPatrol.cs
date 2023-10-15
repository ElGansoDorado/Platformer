using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirPatrol : Enemy
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;

    private float waitTime = 2f;
    private bool canGo = true;

    void Start()
    {
        gameObject.transform.position = new Vector3(point1.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (canGo)
        {
            transform.position = Vector3.MoveTowards(transform.position, point1.position, speed * Time.deltaTime);
        } 

        if (transform.position == point1.position)
        {
            Transform point = point1;
            point1 = point2;
            point2 = point;

            canGo = false;
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);

        transform.eulerAngles = (transform.rotation.y == 0) ? new Vector3(0, 180, 0) : new Vector3(0,0,0);

        canGo = true;   
    }
}
