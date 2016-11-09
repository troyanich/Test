using UnityEngine;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Position Binding")]
public class NguiPositionBinding : NguiPollingVector3Binding
{
		
	protected override Vector3 GetValue()
	{
        return transform.position;
	}
	
	protected override void SetValue(Vector3 val)
	{
		transform.position = val;
	}
}
