using UnityEngine;
using System.Collections;
using EZData;
using UnityEngine.SceneManagement;

public class MapContext : BaseStateContext
{
    #region Property Items
    readonly Collection<MapItemContext> _privateItemsProperty = new Collection<MapItemContext>();
    public Collection<MapItemContext> Items
    {
        get { return _privateItemsProperty; }
    }
    #endregion
    #region Property CurrentIndex
    readonly Property<int> _privateCurrentIndexProperty = new Property<int>();
    public Property<int> CurrentIndexProperty
    {
        get { return _privateCurrentIndexProperty; }
    }
    public int CurrentIndex
    {
        get { return CurrentIndexProperty.GetValue(); }
        set
        {
            CurrentIndexProperty.SetValue(value);
        }
    }
    #endregion
    #region Property CurrentName
    readonly Property<string> _privateCurrentNameProperty = new Property<string>();
    public Property<string> CurrentNameProperty
    {
        get { return _privateCurrentNameProperty; }
    }
    public string CurrentName
    {
        get { return CurrentNameProperty.GetValue(); }
        set
        {
            CurrentNameProperty.SetValue(value);
        }
    }
    #endregion
    public override StateType Type
    {
        get { return StateType.Map; }
    }
    
    private MainContext _mainContext;
    public MapContext(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public void GoToMenu()
    {
        _mainContext.GoToState(StateType.Menu);
    }
    public void OnPlay()
    {
        SceneManager.LoadScene("game");
    }

    public void Select(int i)
    {
        CurrentIndex = i;
        MainManager.instance.CurrentSelectMapId = i;
        CurrentName = MainManager.instance.MapList[i].Name;
    }

    public override void Load()
    {
        Items.Clear();
        for (int i = 0; i < MainManager.instance.MapList.Count; i++)
            Items.Add(new MapItemContext(MainManager.instance.MapList[i].Name, i, this));

        base.Load();
        Select(MainManager.instance.CurrentSelectMapId);
    }
}
