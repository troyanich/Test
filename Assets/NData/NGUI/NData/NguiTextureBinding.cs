using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Texture Binding")]
public class NguiTextureBinding : NguiBinding
{
	protected EZData.Property<Texture2D> _texture;
	protected UITexture _uiTexture;
	protected RawImage _rawImage;
	public bool hideIfEmpty = false;

	private float width;
	private float height;
	
	public enum ALIGNMENT
	{
		STRETCH,
		UNIFORM_STRETCH,
		SOURCE_SIZE,
		NONE
	}
	
	public ALIGNMENT alignment = ALIGNMENT.UNIFORM_STRETCH;
	public bool stretchOutside = false;
	
	public override void Awake()
	{
		base.Awake();
		
		_uiTexture = GetComponent<UITexture>();
		_rawImage = GetComponent<RawImage>();
		if (_uiTexture != null) {
			width = _uiTexture.width;
			height = _uiTexture.height;
		}

		if (_rawImage != null) {
			width = _rawImage.rectTransform.sizeDelta.x;
			height = _rawImage.rectTransform.sizeDelta.y;

		    if (_rawImage.texture == null)
		    {
                _rawImage.color = new Color(_rawImage.color.r, _rawImage.color.g, _rawImage.color.b, 0);             
		    }
		}
            
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		if (_texture != null)
		{
			_texture.OnChange -= OnChange;
			_texture = null;
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
		
		_texture = context.FindProperty<Texture2D>(Path, this);
		
		if (_texture != null)
		{
			_texture.OnChange += OnChange;
		}
	}

	Texture2D _prevTexture;

	protected override void OnChange()
	{
		base.OnChange();
        
		var aspect = (height == 0) ? 1 : (width / height);
		
		var imageWidth = width;
		var imageHeight = height;
		
		if (_texture != null && _texture.GetValue() != null)
		{
			imageWidth = _texture.GetValue().width;
			imageHeight = _texture.GetValue().height;            
		}
		
		var imageAspect = (imageHeight == 0) ? 1 : (imageWidth / imageHeight);
		
		var spriteWidth = 0.0f;
		var spriteHeight = 0.0f;
		
		if (_texture != null && _texture.GetValue() != null)
		{
			switch(alignment)
			{
			case ALIGNMENT.STRETCH:
				spriteWidth = width;
				spriteHeight = height;
				break;
			case ALIGNMENT.UNIFORM_STRETCH:
				if ((aspect > imageAspect) ^ stretchOutside)
				{
					spriteHeight = height;
					spriteWidth = (imageHeight == 0) ? 0 : (imageWidth * spriteHeight / imageHeight);
				}
				else
				{
					spriteWidth = width;
					spriteHeight = (imageWidth == 0) ? 0 : (imageHeight * spriteWidth / imageWidth);
				}
				break;
			case ALIGNMENT.SOURCE_SIZE:
				spriteWidth = imageWidth;
				spriteHeight = imageHeight;
				break;
			}
		}
		
		spriteWidth = Mathf.Max(spriteWidth, 0.001f);
		spriteHeight = Mathf.Max(spriteHeight, 0.001f);
		
		if (_uiTexture != null) {
			if (_texture != null) {
				_uiTexture.mainTexture = _texture.GetValue ();
			}
			if (alignment != ALIGNMENT.NONE) {
				_uiTexture.width = (int)spriteWidth;
				_uiTexture.height = (int)spriteHeight;
			}
		}

		if (_rawImage != null) {
			if (_texture != null) {                
				_rawImage.texture = _texture.GetValue ();
                if(_texture.GetValue()!=null)
                    _rawImage.color = new Color(_rawImage.color.r, _rawImage.color.g, _rawImage.color.b, 1);
                else
                    _rawImage.color = new Color(_rawImage.color.r, _rawImage.color.g, _rawImage.color.b, 0);
			}
            else
                _rawImage.color = new Color(_rawImage.color.r, _rawImage.color.g, _rawImage.color.b, 0);
			if (alignment != ALIGNMENT.NONE) {
				_rawImage.rectTransform.sizeDelta = new Vector2 (spriteWidth, spriteHeight);
			}
		}



		if (_texture != null && _texture.GetValue () == null && _prevTexture != null) {
			_prevTexture = null;
			if(_uiTexture != null)
				_uiTexture.mainTexture = null;
			if(_rawImage != null)
				_rawImage.texture = null;
		}

		if (_texture != null) {
			_prevTexture = _texture.GetValue ();
		}

		if (hideIfEmpty) {
			if (_uiTexture != null && _texture != null)
				Debug.Log (_texture.GetValue ());
				_uiTexture.enabled = _texture.GetValue () != null;
		}
	}
}
