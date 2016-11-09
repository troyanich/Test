using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Local Position Drag Panel Binding")]
public class NguiLocalPositionDragPanelBinding : NguiLocalPositionBinding 
{
	UIPanel _panel;

	public override void Start ()
	{
		base.Start ();
		_panel = GetComponent<UIPanel> ();
	}

	protected override void SetValue (Vector3 val)
	{
		base.SetValue (val);
		if (_panel != null &&  _panel.clipping == UIDrawCall.Clipping.SoftClip)
			_panel.clipRange = new Vector4(0, _panel.clipRange.y, _panel.clipRange.z, _panel.clipRange.w);
	}

}
