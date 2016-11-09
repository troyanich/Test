using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Text Multi Binding")]
public class NguiTextMultiBinding : NguiMultiBinding
{
	public string Format = "";
	
	virtual protected string LocalizedFormat 
	{ 
		get 
		{
			return NguiUtils.LocalizeFormat(Format);
		} 
	} 
	
	private readonly List<Dictionary<Type, EZData.Property>> _propertyGroups = new List<Dictionary<Type, EZData.Property>>();
	
	private bool _ignoreChanges = false;
	
	private UIInput _UiInputReceiver;
	private UILabel _UiLabelReceiver;
	private InputField _inputField;
	private Text _text;
	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);
	
	[HideInInspector]
	public event OnValueChangeDelegate OnValueChange;
	
	public override void Awake()
	{
		base.Awake();
		
		for (var i = 0; i < Paths.Count; ++i)
		{
			_propertyGroups.Add(new Dictionary<Type, EZData.Property>());
		}
		_UiInputReceiver = GetComponent<UIInput>();
		_UiLabelReceiver = GetComponent<UILabel>();
		_inputField = GetComponent<InputField> ();
		_text = GetComponent<Text>(); 
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var g in _propertyGroups)
		{
			foreach(var p in g)
			{
				p.Value.OnChange -= OnChange;
			}
			g.Clear();
		}
			
	}
	
	protected override void Bind()
	{
		base.Bind();
	
		for (var i = 0; i < _propertyGroups.Count && i < Paths.Count; ++i)
		{
			FillTextProperties(_propertyGroups[i], Paths[i]);
		}
		
		foreach(var g in _propertyGroups)
		{
			foreach(var p in g)
			{
				p.Value.OnChange += OnChange;
			}
		}
	}
	
	protected object [] GetRawValues()
	{
		var newValues = new List<object>();
		
		foreach(var g in _propertyGroups)
		{
			newValues.Add(GetTextValue(g));
		}
		
		return newValues.ToArray();
	}
	
	protected string GetValue()
	{
		return string.Format(LocalizedFormat, GetRawValues());
	}
	
	protected override void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		var newValue = GetValue();
		
		if (OnValueChange != null)
		{
			OnValueChange(newValue);
		}
		
		if (_UiInputReceiver != null)
		{
#if NGUI_2
			_UiInputReceiver.text = newValue;
#else
			_UiInputReceiver.value = newValue;
#endif
		}
		
		if (_UiLabelReceiver != null)
		{
			_UiLabelReceiver.text = newValue;
		}

		if (_inputField != null) {
			_inputField.text = newValue;
		}

		if (_text != null) {
			_text.text = newValue;
		}
	}
}
