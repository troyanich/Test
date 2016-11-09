using UnityEngine;
using System.Collections;

public class MainViewModel : MonoBehaviour
{

    public NguiRootContext View;
    public MainContext Context;

    private void Awake()
    {
        Context = new MainContext();
        View.SetContext(Context);

        Context.GoToState(StateType.Menu);
    }
}
