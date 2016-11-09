using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Animation Binding")]
public class NguiAnimationBinding : NguiBinding 
{
	private EZData.Property<string> _clip;
	Animation _animation;

	public override void Awake()
	{
		base.Awake ();

		_animation = GetComponent<Animation> ();
	}

	protected override void Unbind()
	{
		base.Unbind();

		if (_clip != null)
		{
			_clip.OnChange -= OnChange;
			_clip = null;
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

		_clip = context.FindProperty<string>(Path, this);

		if (_clip != null)
		{
			_clip.OnChange += OnChange;
		}
	}

	protected override void OnChange()
	{
		base.OnChange();

		if (_clip == null) {
			_animation.Stop ();
		} else if (!string.IsNullOrEmpty (_clip.GetValue ())) {
			_animation.Play (_clip.GetValue ());
		}
	}
		
}
