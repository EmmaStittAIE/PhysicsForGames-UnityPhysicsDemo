using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] parts;

    void Start()
    {
        parts = GetComponentsInChildren<Rigidbody>();

        SetAllKinematics(true);
    }

    public void SetAllKinematics(bool kinematic)
    {
        foreach (Rigidbody p in parts)
        {
            p.isKinematic = kinematic;
        }
    }
}
