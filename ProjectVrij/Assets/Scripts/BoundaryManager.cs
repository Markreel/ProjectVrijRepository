﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoundaryManager : MonoBehaviour
{
    public static BoundaryManager Instance;

    [SerializeField] private int currentBoundaryIndex;
    [SerializeField] private bool boundaryIsActive;
    [SerializeField] private List<Boundary> boundaries = new List<Boundary>();

    public int CurrentBoundaryIndex { get { return currentBoundaryIndex; } }


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
            _bound.PointA = _child.GetChild(0).GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
            _bound.PointB = _child.GetChild(1).GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();

            boundaries.Add(_bound);
        }
    }

    public float ClampDistance(float _distance)
    {
        if (boundaryIsActive)
            return Mathf.Clamp(_distance, boundaries[currentBoundaryIndex].PointA.m_PathPosition, boundaries[currentBoundaryIndex].PointB.m_PathPosition);
        else
            return _distance;
    }
}
