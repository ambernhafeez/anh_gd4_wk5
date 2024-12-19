using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4;
    Rigidbody rb;
    [SerializeField] Transform focalPoint;
    private UnityEngine.Vector3 startingPos;
    public bool hasRepelPowerup = false;
    public bool hasBlastPowerup = false;
    public bool hasBouncePowerup = false;
    [SerializeField] private float powerupStrength = 10;
    public GameObject powerupIndicator;
    public GameObject blastPowerupIndicator;
    public GameObject bouncePowerupIndicator;
    public GameObject projectile;

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
        Vector2 moveDir = new Vector2(horizontalInput, verticalInput).normalized;

        rb.AddForce((focalPoint.forward * moveDir.y + focalPoint.right * moveDir.x) * moveSpeed);
    
        // focal point follows player
        focalPoint.position = transform.position;

        // respawn mechanic
        if (transform.position.y <= -7)
        {
            ReloadScene();
            
        }

        // powerup indicator position with offset
        powerupIndicator.transform.position = transform.position + new Vector3(0,0,0);
        blastPowerupIndicator.transform.position = transform.position + new Vector3(0,0,0);
        bouncePowerupIndicator.transform.position = transform.position + new Vector3(0,1.4f,0);

        powerupIndicator.gameObject.SetActive(hasRepelPowerup);
        blastPowerupIndicator.gameObject.SetActive(hasBlastPowerup); 
        bouncePowerupIndicator.gameObject.SetActive(hasBouncePowerup);
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
            StartCoroutine(RepelPowerUpCountdownRoutine());
        }

        if (collision.CompareTag("Powerup_Blast"))
        {
            hasBlastPowerup = true;
            Destroy(collision.gameObject);
            StartCoroutine(BlastRoutine());
        }

        if (collision.CompareTag("Powerup_Bounce"))
        {
            hasBouncePowerup = true;
            Destroy(collision.gameObject);
            StartCoroutine(BouncePowerUpCountdownRoutine());
            
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

    private IEnumerator RepelPowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(10);
        hasRepelPowerup = false;
    }

    private IEnumerator BlastRoutine()
    {
        // blast every other second 6 times
        for (int i = 0; i < 6; i++)
        {
            // blast every 45 degrees
            for (int spawnAngle = 0; spawnAngle < 360; spawnAngle += 45)
            {
                Instantiate(projectile, transform.position, Quaternion.Euler(0,spawnAngle,0));
            }

            yield return new WaitForSeconds(1);
        }

        hasBlastPowerup = false;
    }

    private IEnumerator BouncePowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(1);
        hasBouncePowerup = false;
    }

}
