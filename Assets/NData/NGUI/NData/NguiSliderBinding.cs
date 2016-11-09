using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Slider Binding")]
public class NguiSliderBinding : NguiPollingCustomBoundsNumericBinding
{
	private UISlider _UiSliderReceiver;
	private UnityEngine.UI.Slider _slider;
	public override void Awake()
	{
		base.Awake();
		_UiSliderReceiver = GetComponent<UISlider>();
		_slider = GetComponent<UnityEngine.UI.Slider> ();
	}
	
	protected override double GetValue()
	{
		if (_UiSliderReceiver != null)
			return _UiSliderReceiver.value;

		if (_slider != null)
			return _slider.value;

		return 0;
	}
	
	protected override void SetValue(double val)
	{
		if (_UiSliderReceiver != null)
		{
			_UiSliderReceiver.value = (float)val;
		}

		if (_slider != null)
		{
			_slider.value = (float)val;
		}
	}
}
