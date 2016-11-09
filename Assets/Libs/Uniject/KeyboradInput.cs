using System;
using Uniject;
using UnityEngine;
using Collision = Uniject.Collision;

/// <summary>
/// A demonstration of testable input using IInput.
/// The scene contains a sphere on a plane that is controlled 
/// using the horizontal and vertical input axes.
/// Inputs are translated to forces applied to the sphere's rigid body.
/// </summary>
public class KeyboradInput : TestableComponent
{
    public Character Character { get; private set; }
    //private IInput input;
    private ICharacterInput _characterInput;

    public KeyboradInput(TestableGameObject obj, ICharacterInput input,
        Character character) : base(obj)
    {
        this.Character = character;
        this._characterInput = input;
    }

    public override void Update()
    {
        if (!GameManager.instance.CanRun)
        {
            return;
        }
        if (_characterInput.MoveLeft && GameManager.instance.CurrentTrack<3)
        {
            Character.obj.transform.Position += Vector3.left*5;
            GameManager.instance.CurrentTrack += 1;
            return;
        }
        else if (_characterInput.MoveRight && GameManager.instance.CurrentTrack > 1)
        {
            GameManager.instance.CurrentTrack -= 1;
            Character.obj.transform.Position += Vector3.right*5;
            return;
        }
        Character.obj.transform.Position += Vector3.forward * Time.deltaTime * GameManager.instance.MyCharacterInfo.Velocity;
    }

    
}