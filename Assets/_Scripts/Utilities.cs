using UnityEngine;

public static class Utilities
{
    public static Vector3 GetRandomPointAtDistance(Vector3 objectPosition, float maxDistance, float minDistance = 0)
    {
        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
        float randomDistance = Random.Range(minDistance, maxDistance);
        
        Vector3 point = objectPosition + rotation * Vector3.forward * randomDistance;

        return point;
    }
}
