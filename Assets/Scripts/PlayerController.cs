using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Text scoreText;
    public float speed;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public Transform enemyLocation;
    public Slider enemyHealth;
    public Image[] hearts;
    private int health=3;
    public float maxSpeed = 10;
    public float upSpeed = 10;
    private int score = 0;
    private int scoreIncrement = 1;
    private bool countScoreState = false;
    private bool invulnerability = false;
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i<hearts.Length;i++) 
        {
            hearts[i].enabled=true;
        }
    }

    private bool onGroundState = true;

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {

            onGroundState = true;
            if (countScoreState == true) {
                score+=scoreIncrement;
                onGroundState = true; // back on ground
                countScoreState = false; // reset score state
                scoreText.text = "Score: " + score.ToString();
            }
        };
    }

    void Update()
    {
    // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
        }
        // Scoring
        if (!onGroundState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = true;
                Debug.Log(score);
            }
        }
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.velocity = Vector2.zero;
        }
        if (Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            countScoreState = true; //check if Gomba is underneath
        }
    }

    // For Mario Bouncing when jumping on Goomba
    public void Bounce() {
        marioBody.AddForce(Vector2.up * upSpeed* 40);
    }

    // Separate Method to remove invulnerability for timed invoke
    void removeInvulnerability() {
        invulnerability = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if ((this.transform.position.y-other.transform.position.y)>1) {
                Debug.Log("Attack Goomba");
                Bounce();
                enemyHealth.value-=20;
                invulnerability = true;
                Invoke("removeInvulnerability",1f);
            } else {
                Debug.Log("Hit by Gomba!");
                if (!invulnerability) 
                {
                    health -= 1;
                    hearts[health].enabled = false;
                }
                invulnerability = true;
                Invoke("removeInvulnerability",1f);
                if (countScoreState) {
                    countScoreState = false;
                    score--;
                }
                if (health == 0)
                {
                    Time.timeScale = 0.0f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }
}
