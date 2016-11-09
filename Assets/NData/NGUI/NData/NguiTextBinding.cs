using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Text Binding")]
public class NguiTextBinding : NguiBinding
{
	public string Format = "{0}";

    public string RegFilter;    
    private Regex _regCalc;
    public bool MultiSpaces = true;
	
	virtual protected string LocalizedFormat 
	{ 
		get 
		{
			return NguiUtils.LocalizeFormat(Format); 
		} 
	} 
	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	private bool _setPropertyMode = false;	
	
	private UIInput _UiInputReceiver;
	private UILabel _UiLabelReceiver;
	private UnityEngine.UI.Text _uiText;
    private TextMesh _meshText;
	private UnityEngine.UI.InputField _uiInputField;

	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);
	
	[HideInInspector]
	public event OnValueChangeDelegate OnValueChange;
	
	public override void Awake()
	{
		base.Awake();
		
		_UiInputReceiver = GetComponent<UIInput>();
		_UiLabelReceiver = GetComponent<UILabel>();
		_uiText = GetComponent<UnityEngine.UI.Text>();
        _meshText = GetComponent<TextMesh>();
		_uiInputField = GetComponent<UnityEngine.UI.InputField>();
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var p in _properties)
		{
			p.Value.OnChange -= OnChange;
		}
		_properties.Clear();
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		FillTextProperties(_properties, Path);
		
		foreach(var p in _properties)
		{
			p.Value.OnChange += OnChange;
		}
	}
	
	protected void SetProperty(string newValue)
	{
        _setPropertyMode = true;
        SetTextValue(_properties, newValue);
        _setPropertyMode = false;
	}
	/*
	void Update()
	{
		if (_UiInputReceiver != null)
		{
#if NGUI_2
			var text = _UiInputReceiver.text;
#else
			var text = _UiInputReceiver.value;
#endif
			if (text != _prevFrameInputText)
			{
				_prevFrameInputText = text;
				_ignoreChanges = true;
				SetValue(text);
				_ignoreChanges = false;
			}
		}
	}
	*/
	protected virtual object GetRawValue()
	{
		return GetTextValue(_properties);
	}
	
	protected virtual string GetValue()
	{
		var newValue = string.Format(LocalizedFormat, GetRawValue());		
		return newValue;
	}
	
	protected override void OnChange()
	{
        base.OnChange();
		
        var newValue = GetValue();

        if (OnValueChange != null)
		{
			OnValueChange(newValue);
		}
		
		ApplyNewValue(newValue);
	}
	
	protected virtual void ApplyNewValue(string newValue)
	{
        SetUIText(newValue);

        if (_UiInputReceiver != null || _uiInputField != null)
            InputProperty(newValue);
	}

    protected virtual void SetUIText(string newValue)
    {
        if (_UiInputReceiver != null)
        {
#if NGUI_2
			_UiInputReceiver.text = newValue;
#else
            _UiInputReceiver.value = newValue;
#endif
        }

        if (_UiLabelReceiver != null)
            _UiLabelReceiver.text = newValue;
        
        if (_uiText != null)
            _uiText.text = newValue;

        if (_meshText != null)
            _meshText.text = newValue;
        
        if (_uiInputField != null)
            _uiInputField.text = newValue;              
    }

    string _prevInputText = "";
    public void InputProperty(string newValue)
	{
        if (!_setPropertyMode && newValue != _prevInputText)
		{
            if (!string.IsNullOrEmpty(RegFilter) && _regCalc == null)
                _regCalc = new Regex(RegFilter);

            if (!string.IsNullOrEmpty(newValue))
            {
                if (_regCalc != null && !_regCalc.IsMatch(newValue))    
                {    
                    newValue = _prevInputText;    
                }

                if (!MultiSpaces)
                {
                    newValue = newValue.Replace("  ", " ");
                }                    

                string tmp = string.Empty;    
                for (int i = 0, n = newValue.Length; i < n; i++)    
                {    
                    tmp += (newValue[i] > 0X500 && newValue[i] != 0XFFFD) ? (char)0XFFFD : newValue[i];    
                }    
                newValue = tmp;                        

                SetUIText(newValue);
            }

            SetProperty(newValue);            
            _prevInputText = newValue;
		}          
	}

}
