using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public Text score;
    public Text lives;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioSource musicSource;
    public AudioClip musicClip;
    public AudioClip soundClip;
    public GameObject wincoinGameObject;
    public GameObject enemies2GameObject;

    private Rigidbody2D rd2d;
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool facingRight = true;
    private int level = 1;

    //Ground Check
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    Animator anim;

    // Start
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives.text = livesValue.ToString();

        anim = GetComponent<Animator>();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        wincoinGameObject.SetActive(false);

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // KeyCodes and Anim
    void Update()
    {

        //Escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        // Idle Anim
        if (hozMovement <= 0 && isOnGround == true)
        {
            anim.SetInteger("State", 0);
        }
        // Run Anim; chose to use rd2d vel mag because hozMovement > 0 only plays Run if moving RIGHT and not LEFT
        if (rd2d.velocity.magnitude > 0  && isOnGround == true)
        {
            anim.SetInteger("State", 1);
        }
      

        // Jump Anim
        if (vertMovement > 0 && isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }

    }

    void FixedUpdate()
    {
        //Ground Check
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        // WIN
        if (scoreValue == 8)
        {
            winTextObject.SetActive(true);

            enemies2GameObject.SetActive(false);
        }
        // LOSE
        if (livesValue == 0)
        {
            loseTextObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (scoreValue == 4 && level == 1)
        {
            Teleport();
        }

        //Plays win soundClip ONLY when the LAST COIN is collected, not upon surface collision
        if (scoreValue == 7)
        {
            wincoinGameObject.SetActive(true);
        }
        if (collision.collider.tag == "WinCoin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            musicSource.loop = false;
            musicSource.clip = soundClip;
            musicSource.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            //Jump code
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }  
        }
    }

    void Teleport()
    {
        transform.position = new Vector2(111f, 0.9f);
        level = 2;
        livesValue = 3;
        lives.text = livesValue.ToString();

        if (facingRight == true)
        {
            Flip();
        }
    }
}