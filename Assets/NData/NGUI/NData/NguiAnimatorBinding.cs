using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Animator Binding")]
public class NguiAnimatorBinding : NguiBinding 
{
	private EZData.Property<Animator> _animatorParameter;

	Animator _animator;

	public override void Awake()
	{
		base.Awake ();

		_animator = GetComponent<Animator> ();
	}

	protected override void Unbind()
	{
		base.Unbind();

		if (_animatorParameter != null)
		{
			_animatorParameter.OnChange -= OnChange;
			_animatorParameter = null;
		}

	}

	protected override void Bind()
	{
		base.Bind();

		var context = GetContext(Path);
		if (context == null)
		{
			Debug.LogWarning("NguiAnimation.UpdateBinding - context is null");
			return;
		}

		_animatorParameter = context.FindProperty<Animator>(Path, this);

		if (_animatorParameter != null)
		{
			_animatorParameter.OnChange += OnChange;
			if(_animator != null)
				_animatorParameter.SetValue (_animator);
		}
	

	}

	protected override void OnChange()
	{
		
	}
}
