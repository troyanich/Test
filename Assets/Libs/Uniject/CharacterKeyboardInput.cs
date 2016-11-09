using Uniject;
using UnityEngine;

public class CharacterKeyboardInput : ICharacterInput
{
    private IInput _input;

    public CharacterKeyboardInput(IInput input)
    {
        _input = input;
    }
    public bool MoveLeft { get { return _input.GetKeyDown(KeyCode.LeftArrow); } }
    public bool MoveRight { get { return _input.GetKeyDown(KeyCode.RightArrow); } }
}