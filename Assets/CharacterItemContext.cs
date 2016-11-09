using UnityEngine;
using System.Collections;
using EZData;

public class CharacterItemContext : Context
{
    #region Property Name
    readonly Property<string> _privateNameProperty = new Property<string>();
    public Property<string> NameProperty
    {
        get { return _privateNameProperty; }
    }
    public string Name
    {
        get { return NameProperty.GetValue(); }
        set { NameProperty.SetValue(value); }
    }
    #endregion

    #region Property Index
    readonly Property<int> _privateIndexProperty = new Property<int>();
    public Property<int> IndexProperty
    {
        get { return _privateIndexProperty; }
    }
    public int Index
    {
        get { return IndexProperty.GetValue(); }
        set { IndexProperty.SetValue(value); }
    }
    #endregion  

    private readonly CharacterContext _characterContext;

    public CharacterItemContext(string name, int index, CharacterContext context)
    {
        _characterContext = context;
        NameProperty.SetValue(name);
        IndexProperty.SetValue(index);
    }

    public void OnClick()
    {
        _characterContext.Select(IndexProperty.GetValue());        
    }
}