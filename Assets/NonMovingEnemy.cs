using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMovingEnemy : MonoBehaviour
{
    private int frameNumber;
    public GameObject player;
    public GameObject chargedShot;
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
        if(Helper.Distance(transform.position.x, transform.position.y, player.transform.position.x, player.transform.position.y) < 10)
        {
            frameNumber++;
            if (frameNumber >= 120)
            {
                frameNumber = 0;

                float xMagnitude = player.transform.position.x - transform.position.x;
                float yMagnitude = player.transform.position.y - transform.position.y;
                float total = 300.0f;
                float xForce = (xMagnitude / Mathf.Abs(yMagnitude)) * total;
                float yForce = (yMagnitude / Mathf.Abs(xMagnitude)) * total;

                if (xForce > total)
                    xForce = total;
                if (yForce > total)
                    yForce = total;
                if (xForce < total * -1.0f)
                    xForce = total * -1.0f;
                if (yForce < total * -1.0f)
                    yForce = total * -1.0f;

                var clone = Instantiate(chargedShot, new Vector3(transform.position.x, transform.position.y - 2, 1), new Quaternion(0, 0, 0, 1));
                clone.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, 0));
            }
        }
    }
}
