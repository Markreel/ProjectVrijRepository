using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoundaryManager : MonoBehaviour
{
    public static BoundaryManager Instance;

    [SerializeField] private Boundary activeBoundary;
    [SerializeField] private List<Boundary> boundaries = new List<Boundary>();

    public Boundary ActiveBoundary { get { return activeBoundary; } }


    private void Awake()
    {
        Instance = Instance ?? (this);

        PopulateBoundaryList();
    }

    void PopulateBoundaryList()
    {
        boundaries.Clear();

        foreach (Transform _child in transform)
        {
            Boundary _bound = new Boundary();
            _bound.PointA = _child.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            _bound.PointB = _child.GetChild(1).GetComponent<CinemachineVirtualCamera>();

            boundaries.Add(_bound);
        }
    }
}
