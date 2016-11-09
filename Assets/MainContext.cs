using System;
using EZData;
using UnityEngine;
using System.Collections;

public class MainContext : Context
{
    public MenuContext Menu { get; set; }
    public CharacterContext Character { get; set; }
    public MapContext Map { get; set; }
    public GameContext Game { get; set; }

    private BaseStateContext _currentState;
    public MainContext()
    {
        Menu = new MenuContext(this);
        Character = new CharacterContext(this);
        Map = new MapContext(this);
    }

    public void GoToState(StateType state)
    {
        if (_currentState != null)
            _currentState.UnLoad();
        switch (state)
        {
                case StateType.Menu:
                _currentState = Menu;
                break;
                case StateType.Character:
                _currentState = Character;
                break;
                case StateType.Map:
                _currentState = Map;
                break;
                case StateType.Game:
                _currentState = Game;
                break;
            default:
                throw new Exception("нет такого стейта "+state);
        }
        _currentState.Load();
    }
}
