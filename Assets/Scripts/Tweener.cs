using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private Tween activeTween;

    // Add a new tween
    public void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (activeTween == null)
        {
            activeTween = new Tween(targetObject, startPos, endPos, duration);
        }
    }

    void Update()
    {
        if (activeTween != null)
        {
            if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
            {
                // Calculate interpolation fraction
                float timeElapsed = Time.time - activeTween.StartTime;
                float t = timeElapsed / activeTween.Duration;


                // Perform linear interpolation
                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);
            }
            else
            {
                // Set final position and clear the active tween.
                activeTween.Target.position = activeTween.EndPos;
                activeTween = null;
            }
        }
    }

    // Check if tween is complete.
    public bool IsTweenComplete()
    {
        return activeTween == null;
    }
}
