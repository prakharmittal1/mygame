using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    private int frameNumber;
    public GameObject player;
    public Rigidbody rigidBody;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        frameNumber = 0;
        health = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ChargedShot"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag.Equals("Bullet"))
        {
            health--;

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        Destroy(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Helper.Distance(transform.position.x, transform.position.y, player.transform.position.x, player.transform.position.y) < 10)
        {
            frameNumber++;

            if (frameNumber >= 120)
            {
                frameNumber = 0;

                if (player.transform.position.x < transform.position.x)
                {
                    rigidBody.AddForce(Vector3.left * 300);
                }
                else
                {
                    rigidBody.AddForce(Vector3.right * 300);
                }
            }
        }

    }
}
