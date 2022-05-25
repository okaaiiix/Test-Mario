using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 1.0f;
    private int moveRight = -2;
    private int moveDown = -1;
    private float upSpeed = 20;
    private Vector2 velocity;
    private Vector2 deathVector;
    private Rigidbody2D enemyBody;
    public Slider enemyHealth;
    private bool onGroundState = false;
    private int respawnCount = 0;
    private bool jumpMode = false;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x; 
        ComputeVelocity();
    }
    void ComputeVelocity(){
        velocity = new Vector2((moveRight)* 5.0f, 0);
    }
    void MoveGomba(){
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Invoke("LandGoomba",1.0f);
        };
        if (col.gameObject.CompareTag("wall"))
        {
            moveRight *= -1;
            ComputeVelocity();
        }
    }

    void LandGoomba(){
        onGroundState = true;
    }

    void JumpGoomba(){
        onGroundState = false;
        Debug.Log("Goomba Jumps");
        enemyBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpMode && onGroundState) {
            onGroundState = false;
            Invoke("JumpGoomba",1.0f);
        }
        if (enemyHealth.value <=0) 
        {
            Vector2 deathVelocity = new Vector2(0, (moveDown)*maxOffset / enemyPatroltime);
            enemyBody.MovePosition(enemyBody.position + deathVelocity * Time.fixedDeltaTime);
            Respawn();
        }
        MoveGomba();
        // else if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        // {// move gomba
        //     MoveGomba();
        // }
        // else{
        //     // change direction
        //     moveRight *= -1;
        //     ComputeVelocity();
        //     MoveGomba();
        // }

        
    }

    void Respawn() {
        enemyBody.position = new Vector2(2.5f,-0.46f);
        respawnCount += 1;
        enemyHealth.value = 100;
        if (respawnCount % 3 == 0)
        {    
            moveRight=2;
            //Activate Jumper
            jumpMode = true;
        } else {
            moveRight*=2;
        }
        onGroundState = false;
        enemyPatroltime/=2;
        ComputeVelocity();
    }
}
