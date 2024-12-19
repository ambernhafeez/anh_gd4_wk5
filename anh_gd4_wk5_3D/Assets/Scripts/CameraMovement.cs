using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    public float yRange = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // make disappearence of mouse off the screen less intrusive while playing
        // these will make mouse invisible and lock it to the game window until Esc is pressed
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseXInput = Input.GetAxis("Mouse X");
        float mouseYInput = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseSensitivity * Time.deltaTime * mouseXInput);

        // use Mathf.Clamp to constrain camera rotation
        Vector3 currentRotation = transform.GetChild(0).localEulerAngles;
        currentRotation.x += mouseSensitivity * Time.deltaTime * -Input.GetAxis("Mouse Y");
        if (currentRotation.x > 180) currentRotation.x -= 360;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -15, 45);
        transform.GetChild(0).localEulerAngles = currentRotation;
    }
}
