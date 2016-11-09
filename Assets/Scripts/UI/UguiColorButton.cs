using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UguiColorButton : MonoBehaviour
{
	public Graphic target;

	public Color normal = new Color(225f / 255f, 200f / 255f, 150f / 255f, 1f);

	/// <summary>
	/// Color to apply on the pressed event.
	/// </summary>

	public Color pressed = new Color(183f / 255f, 163f / 255f, 123f / 255f, 1f);

	public float duration = 0;

    bool down = false;

	public void OnPointerDown(BaseEventData eventData)
	{
        down = true;
        if (target != null)
			TweenColor.Begin(target.gameObject, duration, pressed);
	}

	public void OnPointerUp(BaseEventData eventData)
	{
        down = false;
        if (target != null)
			TweenColor.Begin(target.gameObject, duration, normal);
	}

	public void OnPressedEnter(BaseEventData eventData)
    {
        if (target != null && down)
            TweenColor.Begin(target.gameObject, duration, pressed);
    }

    public void OnPressedExit(BaseEventData eventData)
    {
        if (target != null && down)
            TweenColor.Begin(target.gameObject, duration, normal);
    }


}
