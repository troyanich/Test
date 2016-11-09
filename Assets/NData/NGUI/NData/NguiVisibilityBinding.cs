using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Visibility Binding")]
public class NguiVisibilityBinding : NguiBooleanBinding, IVisibilityBinding
{
	private NguiVisibilityControl _nvc = new NguiVisibilityControl();
	public bool Visible { get { return _nvc.Visible; } }
	public bool ObjectVisible { get { return _nvc.ObjectVisible; } }
	public bool ComponentVisible { get { return _nvc.ComponentVisible; } }
	public void InvalidateParent() { _nvc.InvalidateParent(); }

	UIInput _input;
	private UnityEngine.UI.InputField _uiInputField;

	public override void Awake()
	{
		base.Awake();
		_nvc.Awake(gameObject);
		_input = GetComponent<UIInput> ();
		_uiInputField = GetComponent<UnityEngine.UI.InputField>();
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		_nvc.ApplyNewValue(newValue);
		if (_input != null)
			_input.isSelected = newValue;

		if (_uiInputField != null) {
			_uiInputField.enabled = newValue;
			if (newValue) {
				_uiInputField.MoveTextStart (true);
			}
		}
	}


	#if UNITY_EDITOR

	[ContextMenu("Change UI visible")]
	void VisibleEnable()
	{
		var grapic = gameObject.GetComponentsInChildren <Graphic> ();
		foreach (var g in grapic) {
			g.enabled = !g.enabled;
		}
	}


	[ContextMenu("Add Canvas group")]
	void AddCanvasGroup()
	{
		var objs = gameObject.GetComponentsInChildren <NguiVisibilityBinding> ();
		foreach (var o in objs) {
			var group = o.gameObject.GetComponent <CanvasGroup> ();
			if (group == null) {
				o.gameObject.AddComponent <CanvasGroup> ();
			}
		}


	}

	[ContextMenu("Disable or Enable all visible scripts")]
	void EnableOrDisable()
	{
		var objs = gameObject.GetComponentsInChildren <NguiVisibilityBinding> ();
		foreach (var o in objs) {
			o.enabled = !o.enabled;
		}
	}

	#endif
}
