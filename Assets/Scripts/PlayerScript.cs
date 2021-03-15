﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rd2d;
    public Text score;
    public int scoreValue;
    public int displayScore;
    public Text gameEndText;
    public Text livesText;
    private int lives;
    public AudioSource playerPlayer2;
    public AudioClip hurtNoise1;
    public AudioClip hurtNoise2;
    public AudioClip hurtNoise3;
    public AudioClip hurtNoise4;
    public AudioClip hurtNoise5;
    public AudioClip dieSFX;
    public AudioClip coinGet;  
    public AudioClip bGMusic;
    public AudioClip winSound;
    
    private int hurtNoiseNumber;
    Animator anim;
    
    private bool isFacingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public float jumpForce;
    public Text controls;

 

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = $"Score: {displayScore}";
       gameEndText.text = "";
       lives = 3;
       livesText.text = $"Lives: {lives}";
       displayScore = 0;
       playerPlayer2.clip = bGMusic;
       playerPlayer2.volume = 0.25f;
        playerPlayer2.Play();
        playerPlayer2.loop = true;
        anim = GetComponent<Animator>();
        controls.text = "WASD to Move. ESC to Close.";
       
    }
void Flip()
   {
     isFacingRight = !isFacingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }

public void hurtNoise()
{
    int hurtNoiseNumber = Random.Range(1, 6);
    if (hurtNoiseNumber == 1)
    {
        playerPlayer2.PlayOneShot(hurtNoise1, 1.0f);
    }
    if (hurtNoiseNumber == 2)
    {
        playerPlayer2.PlayOneShot(hurtNoise2, 1.0f);
    }
    if (hurtNoiseNumber == 3)
    {
        playerPlayer2.PlayOneShot(hurtNoise3, 1.0f);
    }
    if (hurtNoiseNumber == 4)
    {
        playerPlayer2.PlayOneShot(hurtNoise4, 1.0f);
    }
    if (hurtNoiseNumber == 5)
    {
        playerPlayer2.PlayOneShot(hurtNoise5, 1.0f);
    }
    
}
    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (isOnGround == false)
        {
            anim.SetBool("isOnGround", false);
        }
        else if (isOnGround == true)
        {
            anim.SetBool("isOnGround", true);
        }

        if (lives == 0)
        {
            speed = 0;
            gameEndText.text = "You Lose!  by David Rodgers";
            
             
        }

        if (isFacingRight == false && hozMovement > 0)
   {
     Flip();
   }
else if (isFacingRight == true && hozMovement < 0)
   {
     Flip();
   }

    if (Input.GetKey(KeyCode.A))

        {


          anim.SetInteger("LeftState", 1);

         }
     else
        {


          anim.SetInteger("LeftState", 0);

         }

         if (Input.GetKey(KeyCode.D))

        {


          anim.SetInteger("State", 1);

         }
     else
        {


          anim.SetInteger("State", 0);

         }

    }
           
private void OnTriggerEnter2D(Collider2D other)
{
     if (other.gameObject.CompareTag("Coin"))
     {
          other.gameObject.SetActive(false);
          scoreValue = scoreValue + 1; 
          displayScore = displayScore + 1;
          score.text = $"Score: {displayScore}";
          playerPlayer2.PlayOneShot(coinGet, 0.8f);

          if (scoreValue == 4)
        {
            transform.position = new Vector2(55.0f, 25.0f);
            controls.text = "";
                        

            if (lives < 3)
            {
                lives = 3;
                livesText.text = $"Lives: {lives}";
            }
             
        }

        if (scoreValue == 8)
        {
            gameEndText.text = "YOU WIN!  by David Rodgers";
            
            playerPlayer2.Stop();
                playerPlayer2.PlayOneShot(winSound, 0.9f);
        }
              
     }
     else if (other.gameObject.CompareTag("Enemy"))
     {
          other.gameObject.SetActive(false);
          lives = lives - 1;
          livesText.text = $"Lives: {lives}"; 
          hurtNoise();

           if (lives == 0)
        {
            controls.text = "";
                playerPlayer2.Stop();
                playerPlayer2.PlayOneShot(dieSFX, 0.9f);
                anim.SetBool("isDead", true);
        }
     }
} 




private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.collider.tag == "Floor" && isOnGround)
        {
            
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

}
