using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

[System.Serializable]
public abstract class NguiPollingCustomBoundsNumericBinding : NguiCustomBoundsNumericBinding
{
	public NguiBindingDirection Direction = NguiBindingDirection.TwoWay;
	public NguiBindingInitialValue InitialValue = NguiBindingInitialValue.TakeFromModel;
	
	private double _prevValue;
	private bool _inited;
	
	public override void Start()
	{
		base.Start();
		
		if (InitialValue == NguiBindingInitialValue.TakeFromView)
		{
			_inited = true;
			_prevValue = GetValue();
			SetCustomBoundsNumericValue(_prevValue);
		}
	}

	IDisposable _updateDisposable = Disposable.Empty;
	void InternalUpdate()
	{
		var newScale = GetValue();
		if (_prevValue != newScale)
		{
			_prevValue = newScale;
			SetCustomBoundsNumericValue(newScale);
			_updateDisposable.Dispose ();
		}
	}
	
	protected sealed override void ApplyNewCustomBoundsValue(double val)
	{
		if (!_inited && InitialValue == NguiBindingInitialValue.TakeFromView)
			return;
		
		_inited = true;
		if (Direction.NeedsViewUpdate())
		{
			_prevValue = val;
			SetValue(val);
			_updateDisposable = Observable.EveryUpdate ().Where (v => Direction.NeedsViewTracking()).Subscribe (_ => InternalUpdate ());
		}
	}
	
	protected abstract double GetValue();
	protected abstract void SetValue(double val);
}
