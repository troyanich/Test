using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Slider Binding")]
public class UguiScrollbarBinding : NguiPollingCustomBoundsNumericBinding
{
	private UnityEngine.UI.Scrollbar _scrollbar;
	public override void Awake()
	{
		base.Awake();
        _scrollbar = GetComponent<UnityEngine.UI.Scrollbar>();
	}
	
	protected override double GetValue()
	{
        if (_scrollbar != null)
            return _scrollbar.value;

		return 0;
	}
	
	protected override void SetValue(double val)
	{
        if (_scrollbar != null)
		{
            _scrollbar.value = (float)val;
		}
	}
}
