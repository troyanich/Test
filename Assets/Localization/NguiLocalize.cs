using UnityEngine;
using System.Collections;

public class NguiLocalize : MonoBehaviour 
{
	/// <summary>
	/// Localization key.
	/// </summary>

	public string key;
	public string sheetTitle;
	 
	bool mStarted = false;


	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>

	void OnEnable () { if (mStarted) Localize(); }

	/// <summary>
	/// Localize the widget on start.
	/// </summary>

	void Start ()
	{
		mStarted = true;
		Localize();
	}

	/// <summary>
	/// Force-localize the widget.
	/// </summary>

	public void Localize ()
	{
		Localization loc = Localization.instance;
		UIWidget w = GetComponent<UIWidget>();
		UILabel lbl = w as UILabel;
		UISprite sp = w as UISprite;

		// If no localization key has been specified, use the label's text as the key
		if (string.IsNullOrEmpty(key) && lbl != null) key = lbl.text;

		// If we still don't have a key, leave the value as blank
		string val = string.IsNullOrEmpty(key) ? "" : Language.Get(key, sheetTitle);

		if (lbl != null)
		{
			// If this is a label used by input, we should localize its default value instead
			UIInput input = NGUITools.FindInParents<UIInput>(lbl.gameObject);
			if (input != null && input.label == lbl) input.defaultText = val;
			else lbl.text = val;
		}
		else if (sp != null)
		{
			sp.spriteName = val;
			sp.MakePixelPerfect();
		}
	}
}
