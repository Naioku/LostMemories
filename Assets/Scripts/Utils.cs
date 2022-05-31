using UnityEngine;

public static class Utils
{
    public static void MoveTowardsPosition(Vector3 targetPosition, float speed, Transform transform, Rigidbody2D rb)
    {
        Vector2 distanceVector = (targetPosition - transform.position);
        Vector2 directionUnitVector = distanceVector / distanceVector.magnitude;

        rb.velocity = directionUnitVector * speed;
    }
    
    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, digits);
        return Mathf.Round(value * mult) / mult;
    }
}