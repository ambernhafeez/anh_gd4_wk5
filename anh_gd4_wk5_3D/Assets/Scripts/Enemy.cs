using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // move enemy in direction of player by calcualting difference in positions, at a constant speed by normalizing it.
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        // destroy enemy if out of bounds
        if (transform.position.y < -10) { Destroy(gameObject); }
        
    }
}
