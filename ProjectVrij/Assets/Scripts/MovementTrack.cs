using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovementTrack : MonoBehaviour
{
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private int currentPointIndex = 0;

    public Transform CurrentPoint
    {
        get
        {
            if (currentPointIndex < points.Count && points[currentPointIndex] != null)
                return points[currentPointIndex];
            else
            {
                Debug.LogWarning("NULLREFERENCE: Current point in track is empty!");
                return null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        foreach (var _point in points)
        {
            if (points.Count > points.IndexOf(_point) + 1)
            {
                Handles.DrawBezier(_point.position, points[points.IndexOf(_point) + 1].position,
                    _point.GetChild(0).position, _point.GetChild(1).position, Color.yellow, Texture2D.whiteTexture, 5);

                //Handles.SphereCap(0, _point.position, Quaternion.identity, 1);
                //Handles.SphereCap(0, points[points.IndexOf(_point) + 1].position, Quaternion.identity, 1);
                //Handles.SphereCap(0, _point.GetChild(0).position, Quaternion.identity, 0.5f);
                //Handles.SphereCap(0, _point.GetChild(1).position, Quaternion.identity, 0.5f);

                Handles.DrawDottedLine(_point.position, _point.GetChild(0).position, 5);
                //Handles.DrawDottedLine(_point.position, _point.GetChild(1).position, 5);
                //Handles.DrawDottedLine(points[points.IndexOf(_point) + 1].position, _point.GetChild(0).position, 5);
                Handles.DrawDottedLine(points[points.IndexOf(_point) + 1].position, _point.GetChild(1).position, 5);
            }
        }
    }

    public float LenghtOfCurrentCurve()
    {
        List<Vector3> _points = new List<Vector3>();
        float _lenght = 0;

        for (int i = 1; i < 100; i++)
        {
            Vector3 _p = PointOnCurrentCurve(1f / 100f * i);
            _points.Add(_p);
        }

        for (int i = 0; i < _points.Count-1; i++) 
        {
           // Debug.Log(i);
            //Handles.DrawLine(_points[i], _points[i] + new Vector3(0, 2, 0));
            _lenght += Vector3.Distance(_points[i], _points[i + 1]);
        }

        return _lenght;
    }

    public Vector3 PointOnCurrentCurve(float t)
    {
        if (points.Count > currentPointIndex + 1)
        {
            return CubeBezier3(points[currentPointIndex].position, points[currentPointIndex].GetChild(0).position,
                points[currentPointIndex].GetChild(1).position, points[currentPointIndex + 1].position, t);
        }
        else
            return Vector3.zero;
    }

    public Vector3 CubeBezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float r = 1f - t;
        float f0 = r * r * r;
        float f1 = r * r * t * 3;
        float f2 = r * t * t * 3;
        float f3 = t * t * t;
        return f0 * p0 + f1 * p1 + f2 * p2 + f3 * p3;
    }
}
