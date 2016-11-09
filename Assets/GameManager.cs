using System;
using UnityEngine;
using System.Collections;
using Ninject;

public class GameManager : MonoBehaviour
{
    public string Input;
    private LevelController _levelController;
    public GameObject Character;
    public CharacterInfoMy MyCharacterInfo;
    public int CurrentTrack;
    public int _coinsCount;
    public static GameManager instance;
    public event Action CoinsChanged;
    public event Action Finish;
    private bool _canRun;
    public LevelController LevelController
    {
        get { return _levelController; }
    }

    public int CoinsCount
    {
        get { return _coinsCount; }
        set
        {
            _coinsCount = value;
            if (CoinsChanged != null) CoinsChanged();
        }
    }

    public bool CanRun
    {
        get { return _canRun; }
        set { _canRun = value; }
    }

    private void Awake()
    {
        instance = this;
        LoadLevel(MainManager.instance.CurrentSelectMapId.ToString());
        LoadCharacter(MainManager.instance.CharacterList[MainManager.instance.CurrentSelectCharacterId]);
    }

    private void LoadLevel(string nameLevel)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Levels/Level_" + nameLevel));
        _levelController = go.GetComponent<LevelController>();
    }

    private void LoadCharacter(CharacterInfoMy characterInfoMy)
    {
        Character = Instantiate(Resources.Load<GameObject>("Characters/Char" + characterInfoMy.Id));
        MyCharacterInfo = characterInfoMy;
        CurrentTrack = 2;
    }

    public void Start()
    {
        UnityInjector.Get().Get(Type.GetType(Input), new Ninject.Parameters.IParameter[] { });
    }

    public void OnFinish()
    {
        if (Finish != null) Finish();
        CanRun = false;
    }
}
