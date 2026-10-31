using UnityEngine;

public class TurretBuilder : MonoBehaviour
{
    public GameObject turretPrefab;

    public int turretCost = 50;

    void Update()
    {
        // TPS에서만 터렛 설치
        if (CameraSwitch.Instance.IsTPS)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTurret();
            }
        }
    }

    void PlaceTurret()
    {
        Ray ray = CameraSwitch.Instance.TPSCam.ScreenPointToRay(
            Input.mousePosition
        );

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            TurretSpot spot = hit.collider.GetComponent<TurretSpot>();

            if (spot == null)
                return;

            if (spot.isOccupied)
                return;

            if (GoldManager.instance.SpendGold(turretCost))
            {
                spot.BuildTurret(turretPrefab);
            }
        }
    }
}