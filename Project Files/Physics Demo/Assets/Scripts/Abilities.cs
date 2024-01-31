using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    private float shotDistance = 20;
    // i made the ragdolls a bit too heavy, and it's a pain to remake them, so since i already differentiate them below i figured i'd just apply a different force
    private float objectShotForce = 100;
    private float ragdollShotForce = 500;

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, shotDistance);

            Transform containerPart = hit.transform;

            if (containerPart && containerPart.gameObject.layer == LayerMask.NameToLayer("Ragdoll"))
            {
                while (true)
                {
                    if (!containerPart.CompareTag("Ragdoll") && containerPart.parent)
                    {
                        containerPart = containerPart.parent;
                    }
                    else
                    {
                        break;
                    }
                }

                if (containerPart.CompareTag("Ragdoll"))
                {
                    Ragdoll ragdoll = containerPart.GetComponent<Ragdoll>();
                    ragdoll.SetAllKinematics(false);

                    hit.rigidbody.AddForce(cameraTransform.forward * ragdollShotForce);
                }
            }
            else
            {
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(cameraTransform.forward * objectShotForce);
                }
            }
        }
    }
}
