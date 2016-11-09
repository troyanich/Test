using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Alpha Binding")]
public class NguiAlphaBinding : NguiBinding
{
	protected EZData.Property<float> _alpha;

	private CanvasGroup _canvasGroup;
	private Image _image;

    public override void Awake()
    {
        base.Awake();

		_canvasGroup = GetComponent<CanvasGroup> ();
		_image = GetComponent<Image> ();
    }

	protected override void Unbind()
	{
		base.Unbind();

		if (_alpha != null)
		{
			_alpha.OnChange -= OnChange;
			_alpha = null;
		}
	}

	protected override void Bind()
	{
		base.Bind();

		var context = GetContext(Path);
		if (context == null)
		{
			//Debug.LogWarning("NguiAlpha.UpdateBinding - context is null");
			return;
		}

		_alpha = context.FindProperty<float>(Path, this);

		if (_alpha != null)
		{
			//Debug.LogError ("NguiAlpha binding");
			_alpha.OnChange += OnChange;
		}
	}


	protected override void OnChange ()
	{

		base.OnChange ();

		//Debug.Log ("NguiAlpha, CHANGE");

		if (_alpha == null)
			return;

		if (_canvasGroup != null) {
			//Debug.Log ("NguiAlpha, On change");
			_canvasGroup.alpha = _alpha.GetValue ();
		} else if(_image != null){
			_image.CrossFadeAlpha ( _alpha.GetValue (), 0, true);
		}
	}
}
