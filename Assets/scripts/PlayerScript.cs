using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    
    public float speed;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;

    public AudioClip winSound;
    public AudioClip normSound;
    public AudioSource musicSource;

    private int count;
    private int lives;
    private int level;

    private bool winState;
    private bool isOnGround;
    private bool facingRight = true; 
    
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();


        count = 0;
        lives = 3;
        level = 1;

        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        winState = false;
        musicSource.clip = normSound;
        musicSource.Play();

        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

         if (hozMovement == 0 && isOnGround)
        {
            anim.SetInteger ("State" , 0);
        }
        
        if(hozMovement > 0  && isOnGround)
        {
            anim.SetInteger ("State" , 1);
        }

        if (isOnGround == false)
        {
            anim.SetInteger ("State" , 2);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

    }

    void Flip()
        {
            facingRight = !facingRight;
            Vector2 Scaler = transform.localScale;
            Scaler.x = Scaler.x * -1;
            transform.localScale = Scaler;
        }
    
    void SetCountText()
    {
        countText.text = "Count:" + count.ToString();
        livesText.text = "Lives:" + lives.ToString();
        if (count >= 8)
        {
            winTextObject.SetActive(true);
            PlayWin();

        }
        else if (lives <= 0)
        {
            loseTextObject.SetActive(true);
        }
    }

    void PlayWin()
    {
        if(winState == false)
        {
            winState = true;
            musicSource.clip = winSound;
            musicSource.Play();
         }
     }





    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            count = count + 1;
            Destroy(collision.collider.gameObject);
            SetCountText();
        }
          
        else if (collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            SetCountText();
        }

       if (count == 4 && level == 1)
        {
            Teleport();
        }
    }

    void Teleport()
    {
        transform.position = new Vector2(23.9f, 0.0f);
        level = 2;
        lives = 3; 
        SetCountText();
    }
    

    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isOnGround = true;
            if (Input.GetKey(KeyCode.W))
            {
                isOnGround = false;
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }

        void Update()
    { if (Input.GetKey("escape"))
        {
        Application.Quit();
        }
    }
}

