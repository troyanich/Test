using UnityEngine;
using System.Collections;

public class NguiTextureShaderBinding : NguiBinding 
{

	private EZData.Property<Shader> _shader;
	private UITexture _uiTexture;

	public override void Awake()
	{
		base.Awake();

		_uiTexture = gameObject.GetComponent<UITexture>();
	}

	protected override void Unbind()
	{
		base.Unbind();

		if (_shader != null)
		{
			_shader.OnChange -= OnChange;
			_shader = null;
		}
	}

	protected override void Bind()
	{
		base.Bind();

		var context = GetContext(Path);
		if (context == null)
		{
			Debug.LogWarning("NguiTexture.UpdateBinding - context is null");
			return;
		}

		_shader = context.FindProperty<Shader>(Path, this);

		if (_shader != null)
		{
			_shader.OnChange += OnChange;
		}
	}

	protected override void OnChange()
	{
		base.OnChange();

		if (_uiTexture &&_shader != null && _shader.GetValue() != null)
		{
			_uiTexture.shader = _shader.GetValue ();
		}

	}
}
