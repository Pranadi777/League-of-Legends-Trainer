using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Abilities : MonoBehaviour
{
    [Header("Ability1")]
    public Canvas ability1Canvas;
    public Image ability1Skillshot;



    private Vector3 position;
    private RaycastHit hit;
    private Ray ray;


    public GameObject projectilePrefab;
    private Vector3 spawnerPos;
    private Transform spawnerTransform;
    [SerializeField] float bulletSpeed;
    [SerializeField] private float time;
    [SerializeField] private float coolDownTime;
    public bool canShoot = true;






    // Start is called before the first frame update
    void Start()
    {
        // Start of the game, disable the skillshot  image
        ability1Skillshot.enabled = false;
        // also disable the canvas of the skillshot
        ability1Canvas.enabled = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        // check for ability1Input
        Ability1Input();

        // Establish the camera ray
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Ability1Canvas();

        spawnerTransform = GameObject.FindGameObjectWithTag("Spawner").transform;
        spawnerPos = GameObject.FindGameObjectWithTag("Spawner").transform.position;
        
    }


    private void Ability1Input()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ability1Canvas.enabled = true;
            ability1Skillshot.enabled = true;

            Cursor.visible = false;
        }

        if (Input.GetKeyUp(key: KeyCode.Q))
        {
            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;

            Cursor.visible = true;

            if (canShoot)
            {
                StartCoroutine(ShootBullet());
            }
   
        }

        if (ability1Skillshot.enabled && Input.GetMouseButtonDown(0))
        {
            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;

            Cursor.visible = true;
        }


    }
    private void Ability1Canvas()
    {
        if (ability1Skillshot.enabled)
        {
            if (Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab1Canvas = Quaternion.LookRotation(position - transform.position);
            ab1Canvas.eulerAngles = new Vector3 (0, ab1Canvas.eulerAngles.y, ab1Canvas.eulerAngles.z);

            ability1Canvas.transform.rotation = Quaternion.Lerp(ab1Canvas, ability1Canvas.transform.rotation, 0);
        }
    }


    public IEnumerator ShootBullet()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnerPos, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = spawnerTransform.forward * bulletSpeed;
        StartCoroutine(CoolDown());
        yield return new WaitForSeconds(time);
        Destroy(projectile);
    }

    public IEnumerator CoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(coolDownTime);
        canShoot = true;
    }

}
