using Uniject;
using UnityEngine;

public class CharacterMobileInput : TestableComponent, ICharacterInput
{
    private IInput _input;

    public CharacterMobileInput(TestableGameObject go, IInput input)
        : base(go)
    {
        _input = input;
    }

    public bool MoveLeft
    {
        get
        {
            //if (_input.touchCount == 0)
            //    return false;
            //return _input.GetTouch(0).deltaPosition.x < 150;
//            if (!swiping)
//            {
//                return false;
//            }
            return isLeft;
        }
    }

    public bool MoveRight
    {
        get
        {
            //if (_input.touchCount == 0)
            //    return false;
            //return _input.GetTouch(0).deltaPosition.x > 150;
//            if (!swiping)
//            {
//                return false;
//            }
            return isRight;
        }
    }

    private bool swiping = false;
    private Vector2 lastPosition;
    private bool isRight;
    private bool isLeft;

    public override void Update()
    {
        base.Update();
        isRight = false;
        isLeft = false;
        if (_input.touchCount == 0)
            return;

        if (_input.GetTouch(0).phase == TouchPhase.Began)
        {
            lastPosition = _input.GetTouch(0).position;
        }
        else if (_input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 direction = Input.GetTouch(0).position - lastPosition;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    isRight = true;
                    isLeft = false;
                }
                else if (direction.x < 0)
                {
                    isRight = false;
                    isLeft = true;
                }
            }
        }

        /*if (_input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
        {
            if (swiping == false)
            {
                swiping = true;
                lastPosition = _input.GetTouch(0).position;
                return;
            }
            else
            {
                Vector2 direction = Input.GetTouch(0).position - lastPosition;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                    {
                        isRight = true;
                        isLeft = false;
                    }
                    else if (direction.x < 0)
                    {
                        isRight = false;
                        isLeft = true;
                    }
                }
            }
        }
        else
        {
            swiping = false;
        }*/
    }
}
