using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UniRx;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnClick Binding")]
public class NguiOnClickBinding : NguiCommandBinding
{
	const int minDelayBetweenTapsFrames = 15;

	Selectable _selectable;
	int _instanceId;
    public override void Awake()
    {
        base.Awake();
		_instanceId = gameObject.GetInstanceID ();
		_selectable = GetComponent<Selectable> ();

		var input = _selectable as InputField;

		if (input != null) {
			input.onEndEdit.AddListener((text)=> InternalClick());
			return;
		}

		if (_selectable == null) {
			_selectable = gameObject.AddComponent<Button>();
			_selectable.interactable = true;
			_selectable.transition = Selectable.Transition.None;
		}

		var button = _selectable as Button;

		if (button != null) {
			button.onClick.AddListener(InternalClick);
		}
			
    }
		
    public void OnClick()
	{
//		if (_command == null)
//		{
//			return;
//		}
//		_command.DynamicInvoke();
	}
		
	IDisposable _tapDisposable = null;
		
	public void InternalClick()
	{
		if (_tapDisposable != null)
			return;

		if (_command == null)
		{
			return;
		}

		_tapDisposable = Observable.TimerFrame (minDelayBetweenTapsFrames).Subscribe (_ => _tapDisposable = null);

		_command.DynamicInvoke();
	}

    void OnClickInTime()
    {
        StartCoroutine(StartInTime());
    }

    IEnumerator StartInTime() 
    {
        yield return new WaitForSeconds(0.2f);
		InternalClick ();
    }
}
