using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
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
            Vector3Int position = FloorGen.GetFloorPositions()[Random.Range(0, FloorGen.GetFloorPositions().Count)];
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
