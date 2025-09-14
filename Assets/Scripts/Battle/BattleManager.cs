using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(BattleBlackboard))]
public class BattleManager : MonoBehaviour
{
    public GameObject TankPrefab; 
    public string Player0Agent; 
    public string Player1Agent;
    public BattleBlackboard blackboard;
    public List<Transform> SpawnPoints;
    public int nextPlayerID;
    private Dictionary<int, GameObject> _PlayerGOs;
    private Dictionary<int, int> _PlayerPoints;
    public GameObject EndPannel;
    public List<Text> PointsText;
    public List<Text> BehaviourText;
    public List<Transform> Bases;
    public Dictionary<int, object> GetInformation(EBlackboardInformationType type)
    {
        return blackboard.Information[type];
    }

    public void UpdateUI()
    {
        for (int i = 0; i < _PlayerGOs.Count; i++)
        {
            BehaviourText[i].text=$"Player {i}: {_PlayerGOs[i].GetComponent<TankAgent>().GetDescription()}";
        }
    }
    
    public void UpdateBlackboard()
    {
        blackboard.Information[EBlackboardInformationType.playerHp][0] = _PlayerGOs[0].GetComponent<Tank>()._CurrentHp;
        blackboard.Information[EBlackboardInformationType.playerHp][1] = _PlayerGOs[1].GetComponent<Tank>()._CurrentHp;
        
        blackboard.Information[EBlackboardInformationType.playerPosition][0]=_PlayerGOs[0].transform.position;
        blackboard.Information[EBlackboardInformationType.playerPosition][1]=_PlayerGOs[1].transform.position;

        blackboard.Information[EBlackboardInformationType.playerPoints][0] = _PlayerPoints[0];
        blackboard.Information[EBlackboardInformationType.playerPoints][1] = _PlayerPoints[1];
    }

    private void JudgeByHp()
    {
        foreach (var go in _PlayerGOs)
        {
            var tank = go.Value.GetComponent<Tank>();
            if (tank._CurrentHp <= 0)
            {
                tank.ReSpawn(SpawnPoints[go.Key].position);
                for (int i = 0; i < _PlayerPoints.Count; i++)
                {
                    if (i == go.Key)
                    {
                        continue;
                    }

                    _PlayerPoints[i]++;
                    PointsText[i].text=$"Player {i}: {_PlayerPoints[i]} pts";
                }
                
            }
        }
        foreach (var pointPair in _PlayerPoints)
        {
            if (_PlayerPoints[pointPair.Key] > 4)
            {
                EndPannel.GetComponent<Text>().text = $"Player {pointPair.Key} Won!";
                EndPannel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    private void Awake()
    {
        _PlayerGOs = new Dictionary<int, GameObject>();
        _PlayerPoints = new Dictionary<int, int>();
    }
    private void Start()
    {
        
        if (SpawnPoints.Count != 2)
        {
            return;
        }
        blackboard = GetComponent<BattleBlackboard>();
        
        nextPlayerID = 0;
        _PlayerGOs.Add(nextPlayerID,
            Instantiate(TankPrefab, SpawnPoints[nextPlayerID].position, SpawnPoints[nextPlayerID].rotation));
        _PlayerGOs[nextPlayerID].GetComponent<Tank>().PlayerID = nextPlayerID;
        _PlayerPoints.Add(nextPlayerID, 0);
        _PlayerGOs[nextPlayerID].AddComponent ( Type.GetType(Player0Agent));
        AIAgent aiagent = null;
        _PlayerGOs[nextPlayerID].TryGetComponent<AIAgent>(out aiagent);
        if (aiagent != null)
        {
            aiagent.BasePosition=Bases[nextPlayerID].position;
            aiagent = null;
        }
        nextPlayerID++;
        _PlayerGOs.Add(nextPlayerID,
            Instantiate(TankPrefab, SpawnPoints[nextPlayerID].position, SpawnPoints[nextPlayerID].rotation));
        
        _PlayerGOs[nextPlayerID].GetComponent<Tank>().PlayerID = nextPlayerID;
        _PlayerPoints.Add(nextPlayerID, 0);
        _PlayerGOs[nextPlayerID].TryGetComponent<AIAgent>(out aiagent);
        _PlayerGOs[nextPlayerID].AddComponent ( Type.GetType(Player1Agent));
        if (aiagent != null)
        {
            aiagent.BasePosition=Bases[nextPlayerID].position;
        }
        PointsText[0].text=$"Player 0: 0 pts";
        PointsText[1].text=$"Player 1: 0 pts";
        UpdateBlackboard();
        _PlayerGOs[0].GetComponent<TankAgent>().StartAgent();
        _PlayerGOs[1].GetComponent<TankAgent>().StartAgent();
        
    }


    private void Update()
    {
        JudgeByHp();
        UpdateBlackboard();
        UpdateUI();
    }
    
}
