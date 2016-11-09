using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainManager : MonoBehaviour
{
    public List<CharacterInfoMy> CharacterList = new List<CharacterInfoMy>();
    public List<MapInfo> MapList = new List<MapInfo>();
    private int _currentSelectCharacterId;
    private int _currentSelectMapId;
    public static MainManager instance;

    public int CurrentSelectCharacterId
    {
        get { return _currentSelectCharacterId; }
        set { _currentSelectCharacterId = value; }
    }

    public int CurrentSelectMapId
    {
        get { return _currentSelectMapId; }
        set { _currentSelectMapId = value; }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetCharacters();
        CurrentSelectCharacterId = 0;
        SetMaps();
        CurrentSelectMapId = 0;
    }

    private void SetCharacters()
    {
        CharacterList.Add(new CharacterInfoMy("0", "Красный", 10, Color.red));
        CharacterList.Add(new CharacterInfoMy("1", "Зелёный", 20, Color.green));
        CharacterList.Add(new CharacterInfoMy("2", "Синий", 30, Color.blue));
        CharacterList.Add(new CharacterInfoMy("3", "Жёлтый", 40, Color.yellow));
    }

    private void SetMaps()
    {
        MapList.Add(new MapInfo("0","Карта 1"));
        MapList.Add(new MapInfo("1","Карта 2"));
    }
}

public struct CharacterInfoMy
{
    private string _name;
    private int _velocity;
    private Color _color;
    private string _id;

    public CharacterInfoMy(string id, string name, int velocity, Color color)
    {
        _id = id;
        _name = name;
        _velocity = velocity;
        _color = color;
    }

    public string Name
    {
        get { return _name; }
    }

    public int Velocity
    {
        get { return _velocity; }
    }

    public Color Color
    {
        get { return _color; }
    }

    public string Id
    {
        get { return _id; }
    }
}

public struct MapInfo
{
    private string _name;
    private string _id;

    public MapInfo(string id,string name)
    {
        _name = name;
        _id = id;
    }

    public string Name
    {
        get { return _name; }
    }

    public string Id
    {
        get { return _id; }
    }
}
