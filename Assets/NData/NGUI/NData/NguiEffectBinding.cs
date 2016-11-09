using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Effect Binding")]
public class NguiEffectBinding : NguiBinding
{
	protected EZData.Property<bool> _activateProperty;
	protected ParticleSystem[] _particles;
	protected Renderer[] _renderers;
	protected Animation _animation;
	protected GameObject _go;

	public override void Awake()
	{
		base.Awake();
		
		_particles = GetComponentsInChildren<ParticleSystem> ();
		_renderers = GetComponentsInChildren<Renderer> ();
		_animation = GetComponent<Animation> ();
		_go = gameObject;
	}

	protected override void Unbind()
	{
		base.Unbind();

		if (_activateProperty != null)
		{
			_activateProperty.OnChange -= OnChange;
			_activateProperty = null;
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

		_activateProperty = context.FindProperty<bool>(Path, this);

		if (_activateProperty != null)
		{
			_activateProperty.OnChange += OnChange;
		}
	}

	protected override void OnChange()
	{
		base.OnChange ();

		if (_activateProperty != null) {

			bool val = _activateProperty.GetValue ();

			foreach (var r in _renderers) {
				r.gameObject.SetActive (val);
			}

			foreach (var particle in _particles) {
				if (val) {
					particle.gameObject.SetActive (val);
					particle.Play ();
					if (_animation != null)
						_animation.Play ();
				}
				else {
					particle.Stop ();
					particle.Clear ();
					particle.gameObject.SetActive (val);
				}
			}
		}
	}
}
