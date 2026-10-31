using UnityEngine;

public class TurretSpot : MonoBehaviour
{
    public bool isOccupied = false;

    public void BuildTurret(GameObject turretPrefab)
    {
        if (isOccupied)
            return;

        Instantiate(
            turretPrefab,
            transform.position + Vector3.up * 0.5f,
            Quaternion.identity
        );

        isOccupied = true;
    }
}
