using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnTouch Binding")]
public class NguiOnTouchBinding : NguiCommandBinding
{
	public void OnPress(bool isDown)
	{
		if (_command == null)
		{
			return;
		}
        _command.DynamicInvoke();
	}
}
