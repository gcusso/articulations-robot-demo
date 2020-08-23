using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRandomizer : MonoBehaviour
{
    public GameObject table;
    public float robotMinReach = 0.2f;
    public float robotMaxReach = 0.5f;

    private Bounds tableBounds;
    private float targetY;
    private Rigidbody body;
    private Collider ownCollider;

    void Start()
    {
        tableBounds = table.GetComponent<Collider>().bounds;
        targetY = transform.position.y;
        body = GetComponent<Rigidbody>();
        ownCollider = GetComponent<Collider>();
    }

    public void MoveToRandomPosition()
    {
        // random position (on table, within reach)     
        Vector2 tableTopPoint = RandomReachablePointOnTable();
        Vector3 tableCenter = tableBounds.center;
        float x = tableCenter.x + tableTopPoint.x;
        float z = tableCenter.z + tableTopPoint.y;
        transform.position = new Vector3(x, targetY, z);

        // random rotation
        Vector3 randomRotation = new Vector3(
            0,
            Random.value * 360.0f,
            0);

        transform.rotation = Quaternion.Euler(randomRotation);

        // reset velocity
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

    }

    private Vector2 RandomReachablePointOnTable()
    {
        // for (int i = 0; i < 100; i++)
        {
            Vector2 randomOffset = RandomPoint(robotMinReach, robotMaxReach);
            // bool onTable = PointOnTable(randomOffset);
            // if (onTable)
            // {
            return randomOffset;
            // }
        }

        // throw new System.ArithmeticException("Random point not found");
    }

    private bool PointOnTable(Vector2 point)
    {
        /*  point: The 2D point on table top, relative to center of table top.
         *  Determines if this point would be on the table or not.      
         */
        Vector3 tableExtents = tableBounds.extents;
        float targetRadius = ownCollider.bounds.extents.x;
        float safeXDistance = tableExtents.x - targetRadius;
        float safeZDistance = tableExtents.z - targetRadius;
        float xDistance = Mathf.Abs(point.x);
        float yDistance = Mathf.Abs(point.y);
        bool onTable = (xDistance < safeXDistance) && (yDistance < safeZDistance);
        return onTable;
    }

    private static Vector2 RandomPoint(float minRadius, float maxRadius)
    {
        /*  Picks a 2D point randomly at uniform. Must be between minRadius and
         *  maxRadius from center. Point given relative to center.      
         */
        for (int i = 0; i < 100; i++)
        {
            // pick a random point in a square
            float randomX = (Random.value * maxRadius * 2.0f) - maxRadius;
            float randomY = (Random.value * maxRadius * 2.0f) - maxRadius;
            Vector2 randomPoint = new Vector2(randomX, randomY);

            // compute distance to point
            float d = randomPoint.magnitude;

            // keep only if point is between min and max radius
            if (d > minRadius && d < maxRadius)
            {
                return randomPoint;
            }
        }

        throw new System.ArithmeticException("Random point inside min and max radius not found");
    }

}
