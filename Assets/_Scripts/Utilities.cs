using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utilities
{
    public static IEnumerator EaseObjectToPositionRoutine(Transform objectToMove, 
        Vector3 initialPosition, 
        Vector3 targetPosition, 
        float duration, 
        Func<float, float> easingEquationXZ, 
        Func<float, float> easingEquationY)
    {
        float elapsedTime = 0f;

        if (TryGetGroundPoint(targetPosition, out Vector3 hitPosition)) targetPosition.y = hitPosition.y;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            
            Vector3 easeXZ = Vector3.Lerp(initialPosition, targetPosition, easingEquationXZ(t));
            float easeY = Vector3.Lerp(initialPosition, targetPosition, easingEquationY(t)).y;

            objectToMove.position = new Vector3(easeXZ.x, easeY, easeXZ.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the object ends exactly at the target position
        objectToMove.position = targetPosition;
    }
    
    public static IEnumerator EaseObjectToPositionRoutine(Transform objectToMove, 
        Vector3 initialPosition, 
        Vector3 targetPosition, 
        float duration, 
        Func<float, float> easingEquationXYZ)
    {
        float elapsedTime = 0f;

        if (TryGetGroundPoint(targetPosition, out Vector3 hitPosition)) targetPosition.y = hitPosition.y;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            objectToMove.position = Vector3.Lerp(initialPosition, targetPosition, easingEquationXYZ(t));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the object ends exactly at the target position
        objectToMove.position = targetPosition;
    }

    public static bool TryGetGroundPoint(Vector3 origin, out Vector3 result, int ignoreLayer = 3)
    {
        bool didHit = Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, ignoreLayer);

        result = hitInfo.point;
        
        return didHit;
    }

    public static Vector3 GetRandomPositionAtDistance(Vector3 objectPosition, float maxDistance, float minDistance = 0)
    {
        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
        float randomDistance = Random.Range(minDistance, maxDistance);
        
        Vector3 point = objectPosition + rotation * Vector3.forward * randomDistance;

        return point;
    }
    
    public static List<T> ShuffleList<T> (IEnumerable<T> list) => list.OrderBy(_ => new System.Random().Next()).ToList();
}
