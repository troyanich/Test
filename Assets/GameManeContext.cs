using System;
using UnityEngine;
using System.Collections;
using EZData;

public class GameManeContext : Context
{
    public GameContext Game { get; set; }
    public FinishContext Finish { get; set; }

    private BaseStateContext _currentState;
    public GameManeContext()
    {
        Game = new GameContext(this);
        Finish = new FinishContext(this);
    }

    public void GoToState(StateType state)
    {
        if (_currentState != null)
            _currentState.UnLoad();
        switch (state)
        {
                case StateType.Game:
                _currentState = Game;
                break;
                case StateType.Finish:
                _currentState = Finish;
                break;
            default:
                throw new Exception("нет такого стейта "+state);
        }
        _currentState.Load();
    }
}
