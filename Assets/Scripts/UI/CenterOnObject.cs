using UnityEngine;
using System.Collections;

public class CenterOnObject : MonoBehaviour
{
	public UIScrollView dragPanel;
	private UIGrid _grid;
	void Start()
	{
		if (dragPanel == null)
		{
			dragPanel = NGUITools.FindInParents<UIScrollView>(gameObject);
		}
		_grid = NGUITools.FindInParents<UIGrid>(gameObject);
	}

	public void OnClick ()
	{
		if (dragPanel == null)
			return;
		_grid.SetMinMaxPositions ();
		Vector3 newPos = dragPanel.transform.worldToLocalMatrix.MultiplyPoint3x4 (transform.position);
		newPos = new Vector3(newPos.x, transform.localPosition.y, newPos.z);
		SpringPanel.Begin (dragPanel.gameObject, -newPos, 5f);
	}
}
