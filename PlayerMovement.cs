using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 move;
    private Rigidbody rb;
    [SerializeField] private float speed = 200f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (move * speed * Time.deltaTime));
    }
}
