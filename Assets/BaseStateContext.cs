using EZData;
using UnityEngine;
using System.Collections;

public enum StateType
{
    Menu,
    Character,
    Map,
    Game,
    Finish,
}

public abstract class BaseStateContext : Context {
    public abstract StateType Type { get; }
   
    #region Property IsShow
    readonly Property<bool> _privateIsShowProperty = new Property<bool>();
    public Property<bool> IsShowProperty
    {
        get { return _privateIsShowProperty; }
    }
    public bool IsShow
    {
        get { return IsShowProperty.GetValue(); }
        set { IsShowProperty.SetValue(value); }
    }
    #endregion

    public virtual void Load()
    {
        IsShow = true;
    }

    public virtual void UnLoad()
    {
        IsShow = false;
    }
}
