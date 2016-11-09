using System;
using EZData;
using UnityEngine;
using System.Collections;

public class GameContext : BaseStateContext {
    public override StateType Type
    {
        get { return StateType.Game; }
    }

    #region Property CurrentCoins
    readonly Property<int> _privateCurrentCoinsProperty = new Property<int>();
    public Property<int> CurrentCoinsProperty
    {
        get { return _privateCurrentCoinsProperty; }
    }
    public int CurrentCoins
    {
        get { return CurrentCoinsProperty.GetValue(); }
        set
        {
            CurrentCoinsProperty.SetValue(value);
        }
    }
    #endregion

    private GameManeContext gameManeContext;
    public GameContext(GameManeContext mainContext)
    {
        gameManeContext = mainContext;
    }

    public override void Load()
    {
        GameManager.instance.CoinsChanged += InstanceOnCoinsChanged;
        GameManager.instance.Finish+= InstanceOnFinish;
        base.Load();
    }

    private void InstanceOnFinish()
    {
        gameManeContext.GoToState(StateType.Finish);
    }

    public override void UnLoad()
    {
        GameManager.instance.CoinsChanged -= InstanceOnCoinsChanged;
        GameManager.instance.Finish -= InstanceOnFinish;
        base.UnLoad();
    }

    private void InstanceOnCoinsChanged()
    {
        CurrentCoins = GameManager.instance.CoinsCount;
    }
}
