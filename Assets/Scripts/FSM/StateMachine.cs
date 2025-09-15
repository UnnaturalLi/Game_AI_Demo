
    using System;
    using System.Collections.Generic;
    using UnityEngine;
[Serializable]
    public class StateMachine
    {
        public List<string> StateNames;
        public List<State> States;
        private Dictionary<string, State> _StatesDic;
        public string DefaultStateName;
        private bool _initialized = false;
        private bool _start = false;
        public State CurrentState { get; private set; }
        [NonSerialized]
        public AIAgent agent;
        public void SetCurrentState(State next)
        {
            CurrentState?.OnExit(agent);
            CurrentState = next;
            CurrentState?.OnEnter(agent);
        }
        public State GetState(string StateName)
        {
            State state = null;
            _StatesDic.TryGetValue(StateName, out state);
            return state;
        }
        public void Init(AIAgent agent)
        {
            _StatesDic = new Dictionary<string, State>();
            for (int i = 0; i < StateNames.Count; i++)
            {
                _StatesDic.Add(StateNames[i], States[i]);
            }
            this.agent = agent;
            
            _initialized = true;
        }

        public void Start()
        {
            _start = true;
            SetCurrentState(GetState(DefaultStateName));
        }
        public void OnUpdate()
        {
            if (!_initialized||!_start)
            {
                return;
            }
            if (CurrentState != null)
            {
                var state = CurrentState.OnUpdate(agent,this);
                if (state!=CurrentState)
                {
                    SetCurrentState(state);
                }   
            }
        }
    }
