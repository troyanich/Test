using UnityEngine;
using System.Collections;

public class MenuContext : BaseStateContext {

	public override StateType Type
    {
        get { return StateType.Menu;}
    }

    private readonly MainContext _mainContext;

    public MenuContext(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public void OnCharacter()
    {
        _mainContext.GoToState(StateType.Character);
    }

    public void OnMap()
    {
        _mainContext.GoToState(StateType.Map);
    }
}
