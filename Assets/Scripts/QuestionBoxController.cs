using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class QuestionBoxController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public SpringJoint2D springJoint;
    public GameObject consummablePrefab;
    public SpriteRenderer spriteRenderer;
    public Sprite usedQuestionBox;
    private bool hit = false;
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !hit) {
            hit = true;
            rigidBody.AddForce(new Vector2(0, rigidBody.mass*20), ForceMode2D.Impulse);
            Instantiate(consummablePrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z), Quaternion.identity);
            StartCoroutine(Disabler());
        }
    }

    bool ObjectMovedAndStopped() {
        return Mathf.Abs(rigidBody.velocity.magnitude)<0.01;
    }

    IEnumerator Disabler() {
        if (!ObjectMovedAndStopped()) {
            yield return new WaitUntil(() => ObjectMovedAndStopped());
        }
        spriteRenderer.sprite = usedQuestionBox;
        rigidBody.bodyType = RigidbodyType2D.Static;
        this.transform.localPosition = Vector3.zero;
        springJoint.enabled = false;
    }
}