using UnityEngine;
using System.Collections;

[System.Serializable]
public class NguiStartPlayEffectBinding : NguiBinding 
{
	protected EZData.Property<bool> _activateProperty;
	protected ParticleSystem[] _particles;

	public override void Awake()
	{
		base.Awake();

		_particles = GetComponentsInChildren<ParticleSystem> ();
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
			Debug.LogWarning("NguiStartPlayEffect.UpdateBinding - context is null");
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

            foreach (var particle in _particles) {
                if (val)
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                    particle.Clear();
                }
			}
		}
	}

}
