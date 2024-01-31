using UnityEngine;

// Highly janky movement controller with an input manager integrated. Terrible practice, but it's fine for this
public class MovementController : MonoBehaviour
{
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private Transform cameraTransform;

    private Vector3 velocity;
    private Vector3 acceleration;

    private float inputMouseX;
    private float inputMouseY;
    private float inputHorizontal;
    private float inputVertical;

    private float maxVelocity = 8;
    private float speed = 40;
    private float friction = 20;
    private float pushForce = 50;
    private float jumpForce = 10;
    private float gravity = -20;
    private float turnSpeedX = 3;
    private float turnSpeedY = 3;

    private bool spacePressed = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // get input
        inputMouseX = Input.GetAxis("Mouse X");
        inputMouseY = Input.GetAxis("Mouse Y");
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        spacePressed = Input.GetKey(KeyCode.Space);

        // rotate on input
        cameraTransform.Rotate(0, inputMouseX * turnSpeedY, 0);
        cameraTransform.Rotate(-inputMouseY * turnSpeedX, 0, 0);

        // enforce rotation limits
        Vector3 eulerRot = cameraTransform.rotation.eulerAngles;

        eulerRot.z = 0;

        cameraTransform.rotation = Quaternion.Euler(eulerRot);
    }

    void FixedUpdate()
    {
        acceleration.x = inputHorizontal * speed;
        acceleration.z = inputVertical * speed;

        // apply acceleration
        Vector3 flattenedForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;

        velocity += acceleration.x * cameraTransform.right * Time.fixedDeltaTime;
        velocity += acceleration.z * flattenedForward * Time.fixedDeltaTime;

        if (player.isGrounded)
        {
            if (spacePressed)
            {
                velocity.y = jumpForce;
            }
            else
            {
                velocity.y = 0;
            }

            // apply friction
            float deltaFriction = friction * Time.fixedDeltaTime;

            if (Mathf.Abs(velocity.x) - deltaFriction < 0)
            {
                velocity.x = 0;
            }
            else
            {
                velocity.x -= Mathf.Sign(velocity.x) * deltaFriction;
            }

            if (Mathf.Abs(velocity.z) - deltaFriction < 0)
            {
                velocity.z = 0;
            }
            else
            {
                velocity.z -= Mathf.Sign(velocity.z) * deltaFriction;
            }
        }
        else
        {
            // apply gravity
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        // clamp velocity
        velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
        velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);

        // move player
        player.Move(velocity * Time.fixedDeltaTime);

        if (transform.position.y <= -5)
        {
            transform.position = new(0, 5, 0);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Do collision
        Rigidbody other = hit.rigidbody;

        if (other != null)
        {
            other.AddForce(hit.normal * pushForce);
        }
    }
}
