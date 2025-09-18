using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    public class FSMBasedAIAgent : AIAgent
    {
        private  bool _initialized = false;
        private StateMachine _FSM;
        public void LoadFSM()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
            
            
            /*
            _FSM = new StateMachine();
            _FSM.StateNames = new List<string>();
            _FSM.States = new List<State>();
            
            _FSM.StateNames.Add("MoveToEnemyState");
            _FSM.StateNames.Add("RetreatToBaseState");
            _FSM.StateNames.Add("RetreatFromEnemyState");
            _FSM.States.Add(new MoveToEnemyState());
            _FSM.States.Add(new RetreatToBaseState());
            _FSM.States.Add(new RetreatFromEnemyState{retreatDistance =3f});
            
            // Set up transitions for ChaseEnemyState (index 0)
            _FSM.States[0].transitions = new List<Transition>();
            
            // If HP too low -> RetreatToBaseState
            var hpLow = new HpTooLowCondition();
            hpLow.percentage = 0.5f;
            var t0 = new Transition();
            t0.targetState = "RetreatToBaseState";
            t0.condition = hpLow;
            _FSM.States[0].transitions.Add(t0);
            // If enemy too close -> RetreatFromEnemyState
            var distTooClose = new DistanceTooCloseCondition();
            distTooClose.distance = 2;
            var t1 = new Transition();
            t1.targetState = "RetreatFromEnemyState";
            t1.condition = distTooClose;
            _FSM.States[0].transitions.Add(t1);
            // Otherwise remain in ChaseEnemyState (default)

            // Set up transitions for RetreatToBaseState (index 1)
            _FSM.States[1].transitions = new List<Transition>();
            // If HP not too low -> ChaseEnemyState
            var hpOk = new NotCondition();
            hpOk.condition = hpLow;
            var t2 = new Transition();
            t2.targetState = "MoveToEnemyState";
            t2.condition = hpOk;
            _FSM.States[1].transitions.Add(t2);

            // Set up transitions for RetreatFromEnemyState (index 2)
            _FSM.States[2].transitions = new List<Transition>();
            // If not too close and not low HP -> ChaseEnemyState
            var notTooClose = new NotCondition();
            notTooClose.condition = distTooClose;
            var t3 = new Transition();
            t3.targetState = "MoveToEnemyState";
            t3.condition = notTooClose;
            _FSM.States[2].transitions.Add(t3);
            _FSM.DefaultStateName = "MoveToEnemyState";
            
            _FSM.States[0].StateDescription = "MoveToEnemyState";
            _FSM.States[1].StateDescription = "RetreatToBaseState";
            _FSM.States[2].StateDescription = "RetreatFromEnemyState";
            
           
            
            
            string json = JsonConvert.SerializeObject(_FSM, settings);
            File.WriteAllText("/Users/lixiang/Desktop/FSMDemo.txt", json);
            using (var fs=File.Create("/Users/lixiang/Desktop/FSMDemo.txt"))
            {
                
            }
            File.WriteAllText("/Users/lixiang/Desktop/FSMDemo.txt", json);*/
            TextAsset jsonText= Resources.Load<TextAsset>("FSMDemo");
            _FSM = JsonConvert.DeserializeObject<StateMachine>(jsonText.text,settings);
            _FSM.Init(this);
            _initialized = true;
        }

        public override void StartAgent()
        {
            _FSM.Start();
        }

        public override string GetDescription()
        {
            return _FSM?.CurrentState?.StateDescription;
        }
        
        private void Awake()
        {
            LoadFSM();
        }
        
        private void Update()
        {
            if (_initialized)
            {
                _FSM.OnUpdate();
            }
        }
    }
