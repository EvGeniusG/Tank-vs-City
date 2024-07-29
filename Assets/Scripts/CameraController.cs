using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private Transform tank;
    [SerializeField] private HashSet<Vector3> tilePositions = new HashSet<Vector3>();
    [SerializeField] private float cameraSpeed = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddTile(Transform tile){
        tilePositions.Add(tile.position);
    }

    public void setTank(Transform tank){
        this.tank = tank;
    }



    private void FixedUpdate()
    {
        if(tank == null) return;
        Vector3 nearestPoint = GetNearestPointOnTileLine(tank.position);
        Vector3 cameraPosition = Vector3.Lerp(transform.position, new Vector3(nearestPoint.x, nearestPoint.y, nearestPoint.z), cameraSpeed);
        transform.position = cameraPosition;
    }

    private Vector3 GetNearestPointOnTileLine(Vector3 position)
    {
        // Найти два ближайших тайла
        var nearestTiles = tilePositions.OrderBy(t => Vector3.Distance(position, t)).Take(2).ToArray();
        if (nearestTiles.Length < 2)
        {
            Debug.LogError("Недостаточно тайлов для определения линии.");
            return position; // Возвращаем текущую позицию, если недостаточно тайлов
        }

        Vector3 pointA = nearestTiles[0];
        Vector3 pointB = nearestTiles[1];

        // Найти ближайшую точку на линии между pointA и pointB к позиции танка
        Vector3 nearestPoint = GetNearestPointOnLineSegment(pointA, pointB, position);
        return nearestPoint;
    }

    private Vector3 GetNearestPointOnLineSegment(Vector3 A, Vector3 B, Vector3 P)
    {
        Vector3 AP = P - A;
        Vector3 AB = B - A;

        float ab2 = AB.x * AB.x + AB.z * AB.z;
        float ap_ab = AP.x * AB.x + AP.z * AB.z;
        float t = ap_ab / ab2;

        if (t < 0.0f)
        {
            t = 0.0f;
        }
        else if (t > 1.0f)
        {
            t = 1.0f;
        }

        Vector3 nearestPoint = new Vector3(A.x + AB.x * t, 0, A.z + AB.z * t);
        return nearestPoint;
    }
}
