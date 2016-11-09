using System;
using UniRx;
using UnityEngine;

[System.Serializable]
public abstract class NguiPollingVector3Binding : NguiVector3Binding
{
	public NguiBindingDirection Direction = NguiBindingDirection.TwoWay;
	public NguiBindingInitialValue InitialValue = NguiBindingInitialValue.TakeFromModel;
	
	private Vector3 _prevValue;
	private bool _inited;
	
    private IDisposable _updateDisposable = Disposable.Empty;

	public override void Start()
	{
		base.Start();
		
		if (InitialValue == NguiBindingInitialValue.TakeFromView)
		{
			_inited = true;
			_prevValue = GetValue();
			SetVector3Value(_prevValue);
		}

        _updateDisposable = Observable.EveryUpdate().Subscribe(_ => UpdatePosition());
    }
	
	void UpdatePosition()
	{
		if (!Direction.NeedsViewTracking())
			return;
		
		var newScale = GetValue();
		if (_prevValue != newScale)
		{
			_prevValue = newScale;
			SetVector3Value(newScale);
		}
	}

	protected sealed override void ApplyNewValue(Vector3 val)
	{
		if (!_inited && InitialValue == NguiBindingInitialValue.TakeFromView)
			return;
		
		_inited = true;
		if (Direction.NeedsViewUpdate())
		{
			_prevValue = val;
            SetValue(val);
        }
	}

    void OnDestroy()
    {
        _updateDisposable.Dispose();
    }

	protected abstract Vector3 GetValue();
	protected abstract void SetValue(Vector3 val);
}