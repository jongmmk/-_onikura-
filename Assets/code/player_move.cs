using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class player_move : MonoBehaviour
{
    public float playerspeed;
    public float jumpforce;
    public float Dashforce;
    private float dashTimer;
    public float dashTimer_;
    private Rigidbody2D rb;
    private float playermovement;
    private bool isGround;
    private bool isenemyattack;
    private bool isDashing;
    private float originalGravityScale;
    private float savedYVelocity;
    private float player_dir;
    private float dash_cooltime;
    public float dash_cooltime_seting;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        //이동
        if (Keyboard.current.dKey.isPressed) playermovement = 1f;
        else if (Keyboard.current.aKey.isPressed) playermovement = -1f;
        else playermovement = 0f;
        
        //점프
        if (Keyboard.current.wKey.wasPressedThisFrame && isGround)
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            isGround = false;
            Debug.Log("점프!");
        }

        //공격
        if (Keyboard.current.jKey.wasPressedThisFrame && isenemyattack)
        {
            Debug.Log("공격!");
        }

        //대쉬 쿨타임
        if (dash_cooltime > 0f) dash_cooltime -= Time.deltaTime;

        //대쉬
        if (Keyboard.current.shiftKey.wasPressedThisFrame && !isGround && dash_cooltime <= 0f)
        {
            isDashing = true;
            dashTimer = dashTimer_;
            dash_cooltime = dash_cooltime_seting;
            rb.gravityScale = 0f;
            savedYVelocity = rb.linearVelocity.y;
            rb.linearVelocity = new Vector2(player_dir * Dashforce, 0f);
            Debug.Log("대쉬!");
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.gravityScale = originalGravityScale;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, savedYVelocity);
            }
        }
    
    }

    //플레이어가 땅에 있는지 감지
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "block") isGround = true;
        else isGround = false;
    }

    //플레이어 공격 범위에 적이 들어왔는가
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            isenemyattack = true;
        }
        else isenemyattack = false;
    }

    void FixedUpdate()
    {
        if (playermovement != 0f) player_dir = playermovement;

        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(playermovement * playerspeed, rb.linearVelocity.y);
        }
    }
}
