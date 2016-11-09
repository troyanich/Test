using UnityEngine;
using System.Collections;
using EZData;
using UnityEngine.SceneManagement;

public class FinishContext : BaseStateContext
{
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
    public FinishContext(GameManeContext mainContext)
    {
        gameManeContext = mainContext;
    }
    public override StateType Type
    {
        get { return StateType.Finish;}
    }

    public override void Load()
    {
        base.Load();
        CurrentCoins = GameManager.instance.CoinsCount;
    }

    public void OnMenu()
    {
       SceneManager.LoadScene("main");
    }
}
