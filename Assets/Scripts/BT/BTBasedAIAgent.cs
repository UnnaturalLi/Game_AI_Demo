using System;
using Newtonsoft.Json;
using Unity.Collections;
using UnityEngine;
using System.IO;
public class BTBasedAIAgent : AIAgent
{
    private Node _rootNode;
    private bool _running;
    
    public void StopAgent()
    {
        _running = false;
    }
    public void LoadNode()
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };
        
        
        /*_rootNode=new ParallelNode();
        _rootNode.SetCondition(new TrueCondition());
        RepeatNode repeatMoveNode=new RepeatNode{RepeatTimes = -1};
        RepeatNode repeatShootNode=new RepeatNode{RepeatTimes =-1};
        _rootNode.AddChildren(repeatMoveNode, repeatShootNode);
        repeatMoveNode.SetCondition(new TrueCondition());
        repeatShootNode.SetCondition(new TrueCondition());
        SelectNode moveNode=new SelectNode();
        SelectNode shootNode=new SelectNode();
        repeatMoveNode.AddChildren(moveNode);
        repeatShootNode.AddChildren(shootNode);
        
        moveNode.SetCondition(new TrueCondition());
        shootNode.SetCondition(new TrueCondition());
        RotateToEnemyActionNode subRotateNode =new RotateToEnemyActionNode{Description = "Rotate to Enemy"};
        subRotateNode.SetCondition(new NotCondition{condition= new AimAtEnemyCondition()});
        ShootActionNode subShootNode=new ShootActionNode{Description = "Shooting"};
        subShootNode.SetCondition(new AimAtEnemyCondition());
        shootNode.AddChildren(subRotateNode,subShootNode); 
        
        
        BackToBaseActionNode backToBaseActionNode = new BackToBaseActionNode{Description = "Retreat to Base"};
        backToBaseActionNode.SetCondition(new HpTooLowCondition() { percentage = 0.5f });
        MoveToEnemyActionNode moveToEnemyActionNode = new MoveToEnemyActionNode{Description = "Moving Towards Enemy"};
        moveToEnemyActionNode.SetCondition(new NotCondition(){condition = new HpTooLowCondition() { percentage = 0.5f }});
        moveNode.AddChildren(backToBaseActionNode, moveToEnemyActionNode);
        string json=JsonConvert.SerializeObject(_rootNode,settings);
        using (var fs=File.Create("/Users/lixiang/Desktop/BTDemo.txt"))
        {
                
        }
        File.WriteAllText("/Users/lixiang/Desktop/BTDemo.txt", json);*/
        
        TextAsset jsonText= Resources.Load<TextAsset>("BTDemo");
        _rootNode = JsonConvert.DeserializeObject<Node>(jsonText.text,settings);
    }
    private void Update()
    {
        if (_running)
        {
            ENodeState? state = _rootNode?.Update(this);
            if (state != null && state.Value == ENodeState.Failed)
            {
                StopAgent();
            }
        }
    }
    
    public override void StartAgent()
    {
        LoadNode();
        _running = true;
    }
    public override string GetDescription()
    {
        return _rootNode?.GetDescription();
    }
}