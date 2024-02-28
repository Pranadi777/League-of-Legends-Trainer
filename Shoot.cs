using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{


    public GameObject projectilePrefab;
    private Vector3 spawnerPos;
    private Transform spawnerTransform;

    private void Start()
    {
        spawnerTransform = GameObject.FindGameObjectWithTag("Spawner").transform;
        spawnerPos = GameObject.FindGameObjectWithTag("Spawner").transform.position;
    }

    public void ShootBullet()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnerPos, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = spawnerTransform.forward * 10;

    }
}
