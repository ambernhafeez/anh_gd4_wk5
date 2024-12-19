using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4;
    Rigidbody rb;
    [SerializeField] Transform focalPoint;
    private UnityEngine.Vector3 startingPos;
    public bool hasRepelPowerup = false;
    [SerializeField] private float powerupStrength = 10;
    public GameObject powerupIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // want movement in forward direction relative to the camera
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        UnityEngine.Vector2 moveDir = new UnityEngine.Vector2(horizontalInput, verticalInput).normalized;

        rb.AddForce((focalPoint.forward * moveDir.y + focalPoint.right * moveDir.x) * moveSpeed);
    
        // focal point follows player
        focalPoint.position = transform.position;

        // respawn mechanic
        if (transform.position.y <= -7)
        {
            ReloadScene();
            
        }

        // powerup indicator position with offset
        powerupIndicator.transform.position = transform.position + new UnityEngine.Vector3(0,0,0);

        powerupIndicator.gameObject.SetActive(hasRepelPowerup);
    }

    // function for reloading the scene 
    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(0); 
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Powerup_Repel")) {
            hasRepelPowerup = true;
            Destroy(collision.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasRepelPowerup == true)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            UnityEngine.Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Player collided with " + collision.gameObject.name + " with powerup set to " + hasRepelPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    private IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasRepelPowerup = false;
    }
}
