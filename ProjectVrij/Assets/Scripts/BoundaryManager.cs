using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoundaryManager : MonoBehaviour
{
    public static BoundaryManager Instance;

    [SerializeField] private int currentBoundaryIndex;
    [SerializeField] private bool boundaryIsActive;
    [SerializeField] private List<Boundary> boundaries = new List<Boundary>();
    [SerializeField] private Player player;

    public int CurrentBoundaryIndex { get { return currentBoundaryIndex; } }


    private void Awake()
    {
        Instance = Instance ?? (this);

        player = InputManager.Instance.gameObject.GetComponent<Player>();

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

            _bound.Blockades[0] = _child.GetChild(0).gameObject;
            _bound.Blockades[1] = _child.GetChild(1).gameObject;

            Debug.Log(_bound.Blockades[0].gameObject.name);

            if (_bound.Blockades[0].gameObject != null)
                _bound.Blockades[0].gameObject.SetActive(false);
            if (_bound.Blockades[1].gameObject != null)
                _bound.Blockades[1].gameObject.SetActive(false);

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

    public void LiftBoundary()
    {
        boundaryIsActive = false;
        boundaries[currentBoundaryIndex].Completed = true;

        Debug.Log("A: " + boundaries[currentBoundaryIndex].PointA.gameObject.name + " | B: " + boundaries[currentBoundaryIndex].PointB.gameObject.name);

        if (boundaries[currentBoundaryIndex].Blockades[0] != null)
            boundaries[currentBoundaryIndex].Blockades[0].SetActive(false);
        if (boundaries[currentBoundaryIndex].Blockades[1] != null)
            boundaries[currentBoundaryIndex].Blockades[1].SetActive(false);

        //Gives player half of his max health back
        player.GainHealth(player.MaxHealth / 2);
    }

    public void CheckIfWithinBoundary(float _curDis)
    {   //CHECK OF SPELER IN DING ZIT EN SPAWN DAN PAS ENEMIES
        foreach (var _boundary in boundaries)
        {
            if (_curDis > _boundary.PointA.m_PathPosition && _curDis < _boundary.PointB.m_PathPosition && !_boundary.Completed)
            {
                boundaryIsActive = true;
                if (_boundary.Blockades[0] != null)
                    _boundary.Blockades[0].SetActive(true);
                if (_boundary.Blockades[1] != null)
                    _boundary.Blockades[1].SetActive(true);

                currentBoundaryIndex = boundaries.IndexOf(_boundary);
                SpawnManager.Instance.CheckEnemyAmount();
            }
        }
    }
}
