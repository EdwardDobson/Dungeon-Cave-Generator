using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
    public  bool EndPointPlaced;
    PlayerMovement m_playerMovement;

    private void Start()
    {
        m_playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        SetExit();
    }
    public void SetExit()
    {
  
        if (FloorGen.GetFloorPositions().Count > 0 && !EndPointPlaced && m_playerMovement.GetPlayerPlaced())
        {
            List<Vector3Int> FloorPoints = new List<Vector3Int>();
            Vector3Int playerMovement = new Vector3Int((int)m_playerMovement.transform.position.x, (int)m_playerMovement.transform.position.y,0);
            for (int i = 0; i < FloorGen.GetFloorPositions().Count; ++i)
            {
                if (Vector3Int.Distance(FloorGen.GetFloorPositions()[i], playerMovement) >20 )
                {
                    FloorPoints.Add(FloorGen.GetFloorPositions()[i]);
                }
            }
            Vector3Int position = FloorPoints[Random.Range(0, FloorPoints.Count)];
            Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
            transform.position = positionReadjusted;
            if(transform.position != m_playerMovement.transform.position)
            EndPointPlaced = true;
            Debug.Log("Setting exit : " + transform.position);
        }
    }
    public bool GetEndPointSet()
    {
        return EndPointPlaced;
    }
}
