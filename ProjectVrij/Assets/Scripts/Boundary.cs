using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class Boundary
{
    public CinemachineTrackedDolly PointA;
    public CinemachineTrackedDolly PointB;
    public bool Completed = false;

    public GameObject[] Blockades = new GameObject[2];

    // bool onlyUseB "VOOR ALS WE DE SPELER ALLEEN NIET VERDER WILLEN LATEN GAAN MAAR WANNEER HIJ WEL TERUG MAG LOPEN (VOORBIJ A)"
}
