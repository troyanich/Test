using UnityEngine;
using System.Collections;
using UniRx;
using System.Configuration;
using UniRx.Operators;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Opacity Binding")]
public class NguiOpacityBinding : NguiBooleanBinding 
{
	CanvasGroup _group;
	public int DelayFramesWhenOpacityTrue;
	public int DelayFramesWhenOpacityFalse;

	public override void Awake()
	{
		base.Awake();
		_group = GetComponent<CanvasGroup> ();
	}

	protected override void ApplyNewValue(bool newValue)
	{
		int delay = newValue ? DelayFramesWhenOpacityTrue : DelayFramesWhenOpacityFalse;
		SetValue (delay, newValue);
			
	}

	void SetValue(int delay, bool newValue)
	{
		if (delay > 0) {
			Observable.TimerFrame (delay).Subscribe (_ => SetGroup (newValue));
		} else {
			SetGroup (newValue);
		}
	}

	void SetGroup(bool newValue)
	{
		if (_group != null) {
			_group.interactable = newValue;
			_group.blocksRaycasts = newValue;
			_group.alpha = newValue ? 1 : 0;
		}
	}
}
