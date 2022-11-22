using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D Coll;
    public Transform leftpoint, rightpoint;
    public float Speed;
    private bool Faceleft = true;
    private float leftx, rightx;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Faceleft)//面向左侧
        {
            rb.velocity = new Vector2(-Speed, 1);
            if (transform.position.x < leftx)
            {              
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;
            }
            
        }
        else if(!Faceleft)
        {
            rb.velocity = new Vector2(Speed, 1);
        }
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1); 
                Faceleft = true;          
            }
    }
}
