using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Animator Binding")]
public class NguiCameraBinding : NguiBinding
{
    private EZData.Property<Camera> _cameraParameter;

    Camera _camera;

    public override void Awake()
    {
        base.Awake();

        _camera = GetComponent<Camera>();
    }

    protected override void Unbind()
    {
        base.Unbind();

        if (_cameraParameter != null)
        {
            _cameraParameter.OnChange -= OnChange;
            _cameraParameter = null;
        }

    }

    protected override void Bind()
    {
        base.Bind();

        var context = GetContext(Path);
        if (context == null)
        {
            Debug.LogWarning("NguiCamera.UpdateBinding - context is null");
            return;
        }

        _cameraParameter = context.FindProperty<Camera>(Path, this);

        if (_cameraParameter != null)
        {
            _cameraParameter.OnChange += OnChange;
            if (_camera != null)
                _cameraParameter.SetValue(_camera);
        }


    }

    protected override void OnChange()
    {

    }
}
