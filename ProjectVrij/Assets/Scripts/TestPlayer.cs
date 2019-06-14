using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] MovementTrack movementTrack;
    [SerializeField] float speed = 5;
    [SerializeField] float t = 0;
    [SerializeField] bool inverted = false;

    private Vector3 lastPos;

    void Update()
    {
        lastPos = transform.position;

        if (!inverted)
            if (t < 1)
                t += Time.deltaTime / movementTrack.LenghtOfCurrentCurve() * speed;
            else
                t = 0;
        else
            if (t > 0)
                t -= Time.deltaTime / movementTrack.LenghtOfCurrentCurve() * speed;
            else
                t = 1;

        Vector3 _pointPos = movementTrack.PointOnCurrentCurve(t);
        transform.position = new Vector3(_pointPos.x, transform.position.y, _pointPos.z);

        transform.eulerAngles = new Vector3(-lastPos.x, 0, -lastPos.z);
        //Debug.Log(movementTrack.LenghtOfCurrentCurve());
    }
}
