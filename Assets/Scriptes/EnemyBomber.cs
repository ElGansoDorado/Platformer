using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : EnemyAirPatrol
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float timeShoot = 4f;
    void Start()
    {
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(timeShoot);
        Instantiate(bullet, transform.position, transform.rotation);

        StartCoroutine(Shooting());
    }
}
