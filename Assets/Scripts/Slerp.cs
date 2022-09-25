using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://gist.github.com/Matthew-J-Spencer/9044a711cddc4340c6d2aa0656a15d2a

public class Slerp : MonoBehaviour {
    [SerializeField] private Transform _start, _center, _end;
    [SerializeField] private int _count = 15;
    private void OnDrawGizmos() {
        foreach (var point in EvaluateSlerpPoints(_start.position, _end.position, _center.position, _count)) {
            Gizmos.DrawSphere(point, 0.1f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_center.position, 0.2f);
    }

    IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, Vector3 center, int count = 10) {
        var startRelativeCenter = start - center;
        var endRelativeCenter = end - center;

        var f = 1f / count;

        for (var i = 0f; i < 1 + f; i += f) {
            yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + center;
        }
    }
}