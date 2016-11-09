using EZData;
using UnityEngine;
using System.Collections;

public class CharacterContext : BaseStateContext
{
    public override StateType Type
    {
        get { return StateType.Character; }
    }

    #region Property Items
    readonly Collection<CharacterItemContext> _privateItemsProperty = new Collection<CharacterItemContext>();
    public Collection<CharacterItemContext> ItemsProperty
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
            SetPrefab();
        }
    }
    #endregion
    #region Property CurrentVelocity
    readonly Property<int> _privateCurrentVelocityProperty = new Property<int>();
    public Property<int> CurrentVelocityProperty
    {
        get { return _privateCurrentVelocityProperty; }
    }
    public int CurrentVelocity
    {
        get { return CurrentVelocityProperty.GetValue(); }
        set
        {
            CurrentVelocityProperty.SetValue(value);
        }
    }
    #endregion

    private GameObject _prefab;
    private readonly MainContext _mainContext;

    public CharacterContext(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    private void SetPrefab()
    {
        if (_prefab != null)
            Object.Destroy(_prefab);

        _prefab = Object.Instantiate(Resources.Load<GameObject>("Characters/Char" + MainManager.instance.CharacterList[CurrentIndex].Id));
        Transform transform = _prefab.transform;
        transform.localScale = Vector3.one;
        transform.position = Vector3.zero;
    }

    public override void Load()
    {
        ItemsProperty.Clear();
        for (int i = 0; i < MainManager.instance.CharacterList.Count; i++)
            ItemsProperty.Add(new CharacterItemContext(MainManager.instance.CharacterList[i].Name, i, this));

        base.Load();
        Select(MainManager.instance.CurrentSelectCharacterId);
    }

    public override void UnLoad()
    {
        if (_prefab != null)
            Object.Destroy(_prefab);
        base.UnLoad();
    }

    public void Select(int idx)
    {
        MainManager.instance.CurrentSelectCharacterId = idx;
        CurrentIndex = idx;
        CurrentVelocity = MainManager.instance.CharacterList[idx].Velocity;
    }

    public void GoToMenu()
    {
        _mainContext.GoToState(StateType.Menu);
    }
}
