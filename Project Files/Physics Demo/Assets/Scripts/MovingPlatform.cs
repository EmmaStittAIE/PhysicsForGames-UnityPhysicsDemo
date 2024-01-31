using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Rigidbody platform;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;

    private CharacterController player;

    private Vector3 lastPos;

    // Time in seconds that the platform takes to travel from one end to the other
    private float travelTime = 5;
    // Time in seconds that the platform stops after reaching one end
    private float waitTime = 7;
    private float lerpAmount = 0;
    private float waitTimer = 0;

    private bool increasing = true;
    private bool moving = false;

    void Start()
    {
        transform.position = startPoint.position;
    }

    void FixedUpdate()
    {
        lastPos = transform.position;

        if (moving == false)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > waitTime)
            {
                waitTimer = 0;
                moving = true;
            }
        }
        else
        {
            if (increasing)
            {
                lerpAmount += Time.deltaTime;

                if (lerpAmount > travelTime)
                {
                    lerpAmount = travelTime;
                    increasing = false;
                    moving = false;
                }
            }
            else
            {
                lerpAmount -= Time.deltaTime;

                if (lerpAmount < 0)
                {
                    lerpAmount = 0;
                    increasing = true;
                    moving = false;
                }
            }

            float xPos = Mathf.Lerp(startPoint.position.x, endPoint.position.x, lerpAmount / travelTime);
            float yPos = Mathf.Lerp(startPoint.position.y, endPoint.position.y, lerpAmount / travelTime);
            float zPos = Mathf.Lerp(startPoint.position.z, endPoint.position.z, lerpAmount / travelTime);

            Vector3 newPos = new Vector3(xPos, yPos, zPos);

            platform.MovePosition(newPos);

            if (player)
            {
                player.Move(newPos - lastPos);
                // I hate this, but the player is considered "floating" while the platform is moving upward otherwise
                player.Move(new Vector3(0, -0.1f, 0) * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = (CharacterController)other.GetComponent("CharacterController");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
