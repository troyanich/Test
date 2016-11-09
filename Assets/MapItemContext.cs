using UnityEngine;
using System.Collections;
using EZData;

public class MapItemContext : Context {

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

    private readonly MapContext _mapContext;

    public MapItemContext(string name, int index, MapContext context)
    {
        _mapContext = context;
        NameProperty.SetValue(name);
        IndexProperty.SetValue(index);
    }

    public void OnClick()
    {
        _mapContext.Select(IndexProperty.GetValue());
    }
}
