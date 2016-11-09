using UnityEngine;
using System.Collections;

public class GameViewModel : MonoBehaviour {

    public NguiRootContext View;
    public GameManeContext Context;

    private void Awake()
    {
        Context = new GameManeContext();
        View.SetContext(Context);

        Context.GoToState(StateType.Game);
    }
}
