using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MushroomController : MonoBehaviour
{
    public int constantSpeed;
    private Vector2 velocity;
    public Rigidbody2D mushroomBody;
    private int moveRight = -2;
    private bool stop = false;

    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        ComputeVelocity();
        stop = false;
    }
    void ComputeVelocity(){
        velocity = new Vector2((moveRight)* 5.0f, 0);
    }
    void MoveMushroom(){
        mushroomBody.MovePosition(mushroomBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            moveRight *= -1;
            velocity = new Vector2((moveRight)* 5.0f, 0);
            ComputeVelocity();
        }

        if (col.gameObject.CompareTag("Player"))
        {
            stop = true;
        }
    }

    void Update() {
        if (!stop) {
            MoveMushroom();
        }
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }


}