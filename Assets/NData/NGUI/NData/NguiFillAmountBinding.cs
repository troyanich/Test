using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/FillAmout Binding")]
public class NguiFillAmountBinding : NguiCustomBoundsNumericBinding
{
	private float _prevValue = -1.0f;
	public bool twoWay = true;
	
	private UISprite _UiSpriteReceiver;
	private UnityEngine.UI.Image _uiImage;

	public override void Awake()
	{
		base.Awake();
		
		_UiSpriteReceiver = GetComponent<UISprite>();
		_uiImage = GetComponent<UnityEngine.UI.Image>();

	}
	
	void InternalSetValue()
	{
		if (twoWay &&
			_UiSpriteReceiver != null &&
			_prevValue != _UiSpriteReceiver.fillAmount)
		{
			_prevValue = _UiSpriteReceiver.fillAmount;
			SetCustomBoundsNumericValue(_UiSpriteReceiver.fillAmount);
		}

		if (twoWay &&
			_uiImage != null &&
			_prevValue != _uiImage.fillAmount)
		{
			_prevValue = _uiImage.fillAmount;
			SetCustomBoundsNumericValue(_uiImage.fillAmount);
		}
	}
	
	protected override void ApplyNewCustomBoundsValue(double val)
	{
		if (_UiSpriteReceiver != null) {
			_UiSpriteReceiver.fillAmount = (float)val;
			InternalSetValue ();
		}

		if (_uiImage != null) {
			_uiImage.fillAmount = (float)val;
			InternalSetValue ();
		}
			

	}
}
