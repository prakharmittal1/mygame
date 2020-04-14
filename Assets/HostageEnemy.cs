using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageEnemy : MonoBehaviour
{
    public GameObject hostage;
    public GameObject player;
    public GameObject wordBubble;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ChargedShot"))
        {
            Destroy(gameObject);
            Destroy(wordBubble);
        }
        else if (other.gameObject.tag.Equals("Bullet"))
        {
            health--;

            if (health <= 0)
            {
                Destroy(gameObject);
                Destroy(wordBubble);
            }
        }

        Destroy(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Helper.Distance(transform.position.x, transform.position.y, player.transform.position.x, player.transform.position.y) < 5)
        {
            ((SpriteRenderer)GetComponent<SpriteRenderer>()).color = new Color(255, 0, 0);
            Destroy(hostage);
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
    }
}
