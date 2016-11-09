using System.Linq;
using SimpleLogger;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Grayscale Binding")]
public class NguiGrayscaleBinding : NguiBinding 
{
	private EZData.Property<bool> _grayscale;
    public bool deepScanChilds;
	public bool scanTexts;
    private string[] spritesNameToIgnore = { "Notifications", };
	private string[] textsNameToIgnore = {};
    public Transform[] childsToIgonre;
	Image[] _sprites;
	List<Image> sprites = new List<Image>();

	Text[] _texts;
	List<Text> texts = new List<Text>();

	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);

	[HideInInspector]
	public event OnValueChangeDelegate OnValueChange;

	Material _grayScaleMat;
	public override void Awake()
	{
		base.Awake();

		_grayScaleMat = new Material(Shader.Find ("UI/Grayscale"));
	    if (!deepScanChilds)
	    {
			_sprites = GetComponentsInChildren<Image>();
			_texts = GetComponentsInChildren<Text> ();
            if(childsToIgonre.Length == 0) return;
	        var spriteList = _sprites.ToList();
			var textList = _texts.ToList ();
	        foreach (var childTransform in childsToIgonre)
	        {
				var childSprites = childTransform.GetComponentsInChildren<Image>();
	            foreach (var childSprite in childSprites)
	            {
	                spriteList.RemoveAll(s => s == childSprite);
	            }

				if (scanTexts) {
					var childTexts = childTransform.GetComponentsInChildren<Text> ();
					foreach (var childText in childTexts) {
						textList.RemoveAll (t => t == childText);
					}
				}
	        }
	        _sprites = spriteList.ToArray();
			_texts = textList.ToArray ();
	    }
	    else
	    {
	        ScanForAllChilds(transform);
	        _sprites = sprites.ToArray();
			_texts = texts.ToArray();
	        Debug.LogError("Deep scan for childs is finished");
	    }
        
	}
    private void ScanForAllChilds(Transform tr)
    {       
        foreach (Transform child in tr)
        {
			var sprite = child.GetComponent<Image>();
			var text = child.GetComponent<Text> ();
            if (sprite != null)
            {
                bool valid = spritesNameToIgnore.All(s => s != sprite.name);
                if(valid)
                    sprites.Add(sprite);
            }
			if (scanTexts && text != null)
			{
				bool valid = textsNameToIgnore.All(t => t != text.name);
				if (valid)
					texts.Add (text);
			}

            ScanForAllChilds(child);
        }

		Debug.LogError (texts.Count);
    }


	protected override void Unbind()
	{
		base.Unbind();

		if (_grayscale != null)
		{
			_grayscale.OnChange -= OnChange;
			_grayscale = null;
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
	
		_grayscale = context.FindProperty<bool>(Path, this);

		if (_grayscale != null)
		{
			_grayscale.OnChange += OnChange;
		}
	}


	protected override void OnChange()
	{
		base.OnChange();

		if (_grayscale == null)
			return;

	    foreach (var sprite in _sprites) {
			ApplyNewValue(sprite, _grayscale.GetValue ());
		}

		if (scanTexts) {
			foreach (var text in _texts) {
				ApplyNewValue (text, _grayscale.GetValue ());
			}
		}
	}
		


	protected virtual void ApplyNewValue(MaskableGraphic graphic, bool newValue)
	{
        if(childsToIgonre.Any())
            Debug.LogError(graphic.transform.parent.name);

		graphic.material = newValue ? _grayScaleMat : null;

		NguiUtils.SetSelectableEnabled (transform, !newValue, childsToIgonre);
	}
		


}
