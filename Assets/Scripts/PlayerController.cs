using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController :MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    public float jumpForce,speed;
    public Transform groundCheck;
    public LayerMask ground;
    public bool isGround, isJump;  
    bool jumpPressed;
    int jumpCount;
    public int Cherry;
    public int Gem;
    public Text CherryNum;
    public Text GemNum;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if(Input.GetButtonDown("Jump") && jumpCount >0)
        {
            jumpPressed = true;
        }
    }
    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position,0.1f,ground);
      
        
        if (!anim.GetBool("hurt"))
        {
            GroundMovement();
        }

        Jump();

        SwitchAnim();

    }

    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed,rb.velocity.y);

        if(horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove,1,1);
        }
    }

    void Jump()
    {
        if(isGround)
        {
            jumpCount = 2;
            isJump = false;
        }

        if(jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void SwitchAnim()//切换动画效果
    {
        anim.SetFloat("running",Mathf.Abs(rb.velocity.x));

        if(rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",true);
            anim.SetBool("jumping",false);
        }
        else
        {
            anim.SetBool("falling",false);
        }

        if (!anim.GetBool("hurt"))
        {
            if (isGround)
            {
                anim.SetBool("falling", false);
            }
            else if (!isGround && rb.velocity.y > 0)
            {
                anim.SetBool("jumping", true);
            }
            else if (!isGround && rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (anim.GetBool("hurt"))
        {
            anim.SetBool("hurt", true);
            if (Mathf.Abs(rb.velocity.x)<0.1f)
            {
                
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)//收集物品 
    {
        if (collision.tag == "Cherry")
        {
            Destroy(collision.gameObject);
            Cherry++;
            CherryNum.text = Cherry.ToString();
        }
        else if (collision.tag == "Gem")
        {
            Destroy(collision.gameObject); 
            Gem++;
            GemNum.text = Gem.ToString();
        }
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Enemies")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                jumpCount++;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                anim.SetBool("hurt", true);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                anim.SetBool("hurt", true);
            }
        }    
    }

}
