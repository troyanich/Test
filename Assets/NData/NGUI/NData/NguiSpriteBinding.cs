using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleLogger;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Sprite Binding")]
public class NguiSpriteBinding : NguiBinding
{
	public string SpriteSheetPath;
    public string[] SpriteSheetExtraPathes;
    
	public string format = "{0}";
	public bool makePixelPerfect = true;
    public Vector2 rescale = Vector2.one;
   
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	private UISprite _UiSpriteReceiver;
	private UnityEngine.UI.Image _uiImage;
	private SpriteRenderer _spriteRenderer;

	public override void Awake()
	{
		base.Awake();
		
		_properties.Add(typeof(string), null);
		_properties.Add(typeof(int), null);
#if NGUI_2
		_properties.Add(typeof(UIAtlas.Sprite), null);
#else
		_properties.Add(typeof(UISpriteData), null);
#endif
		_properties.Add(typeof(KeyValuePair<string, string>), null);

		_properties.Add(typeof(Sprite), null);

		_UiSpriteReceiver = GetComponent<UISprite>();
		_uiImage = GetComponent<UnityEngine.UI.Image>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange -= OnChange;
				_properties[p.Key] = null;
				break;
			}
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
			
		var context = GetContext(Path);
		if (context == null)
		{
			Debug.LogWarning("NguiSpriteBinding.UpdateBinding - context is null");
			return;
		}
		
		_properties[typeof(string)] = context.FindProperty<string>(Path, this);
		_properties[typeof(int)] = context.FindProperty<int>(Path, this);
#if NGUI_2
		_properties[typeof(UIAtlas.Sprite)] = context.FindProperty<UIAtlas.Sprite>(Path, this);
#else
		_properties[typeof(UISpriteData)] = context.FindProperty<UISpriteData>(Path, this);
#endif
		_properties[typeof(KeyValuePair<string, string>)] = context.FindProperty<KeyValuePair<string, string>>(Path, this);

		_properties[typeof(Sprite)] = context.FindProperty<Sprite>(Path, this);

		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange += OnChange;				
			}
		}
	}
	
	protected override void OnChange()
	{
		base.OnChange();
		
		var newValue = string.Empty;

		if (_properties[typeof(string)] != null)
		{
			newValue = ((EZData.Property<string>)_properties[typeof(string)]).GetValue();
		}
			
			

		if (_properties[typeof(int)] != null)
		{
			newValue = ((EZData.Property<int>)_properties[typeof(int)]).GetValue().ToString ();
		}
#if NGUI_2
		if (_properties[typeof(UIAtlas.Sprite)] != null)
		{
			var sprite = ((EZData.Property<UIAtlas.Sprite>)_properties[typeof(UIAtlas.Sprite)]).GetValue();
			newValue = sprite != null ? sprite.name : string.Empty;
		}
#else
		if (_properties[typeof(UISpriteData)] != null)
		{
			var sprite = ((EZData.Property<UISpriteData>)_properties[typeof(UISpriteData)]).GetValue();
			newValue = sprite != null ? sprite.name : string.Empty;
		}


#endif

		if (_properties[typeof(KeyValuePair<string, string>)] != null)
		{
			SpriteSheetPath = ((EZData.Property<KeyValuePair<string, string>>)_properties [typeof(KeyValuePair<string, string>)]).GetValue ().Value;
			newValue = ((EZData.Property<KeyValuePair<string, string>>)_properties [typeof(KeyValuePair<string, string>)]).GetValue ().Key;
//			Debug.Log ("object " + name + ", SpriteSheetPath = " + SpriteSheetPath + ", newValue = " + newValue);
		}

		if (_properties [typeof(Sprite)] != null) {
            Sprite sprite = ((EZData.Property<Sprite>)_properties[typeof(Sprite)]).GetValue();
            if (sprite == null)
                sprite = SpriteSheetRepository.Instance.GetSpriteByName("empty", "sprites");                
            //float a = sprite != null ? 1F : 0F;                        

            if (_uiImage != null) {
				_uiImage.sprite = sprite;
				_uiImage.color = new Color (_uiImage.color.r, _uiImage.color.g, _uiImage.color.b, 1F);
				return;
			}

			if (_spriteRenderer != null) {
				_spriteRenderer.sprite = sprite;
				_spriteRenderer.color = new Color (_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1F);
				return;
			}
		}


		if (_UiSpriteReceiver != null)
		{
			_UiSpriteReceiver.spriteName = MakeSpriteName(newValue);
		    if (makePixelPerfect)
		    {
		        _UiSpriteReceiver.MakePixelPerfect();
		        if (rescale == Vector2.one) return;

		        //gameObject.transform.localScale = rescale;
                _UiSpriteReceiver.width = (int)(_UiSpriteReceiver.width * rescale.x);
		        _UiSpriteReceiver.height = (int)(_UiSpriteReceiver.height * rescale.y);
		    }
		}

		if (_uiImage != null) {
            Sprite sprite = SpriteSheetRepository.Instance.GetSpriteByName (MakeSpriteName (newValue), SpriteSheetPath);
            if (sprite == null)
                for (int i = 0, len = SpriteSheetExtraPathes.Length; i < len && sprite == null; i++)
                    sprite = SpriteSheetRepository.Instance.GetSpriteByName(MakeSpriteName(newValue), SpriteSheetExtraPathes[i]);
            if (sprite == null && SpriteSheetPath != string.Empty && newValue != " " && !string.IsNullOrEmpty(newValue))
		    {
//		        Debug.LogError("Can't find sprite <" + newValue + "> in " + SpriteSheetPath + " sheet");               
		    }

			_uiImage.sprite = sprite;
		   
	        _uiImage.color = new Color(_uiImage.color.r, _uiImage.color.g, _uiImage.color.b, _uiImage.sprite != null ? 1f : 0f);
	   	

			if (makePixelPerfect)
			{
				_uiImage.SetNativeSize ();
				if (rescale == Vector2.one) return;

				//gameObject.transform.localScale = rescale;
				_uiImage.rectTransform.sizeDelta = new Vector2(_uiImage.rectTransform.sizeDelta.x * rescale.x, _uiImage.rectTransform.sizeDelta.y * rescale.y); 
			}
		}

		if (_spriteRenderer != null) {
			Sprite sprite = SpriteSheetRepository.Instance.GetSpriteByName (MakeSpriteName (newValue), SpriteSheetPath);
			if (sprite == null)
				for (int i = 0, len = SpriteSheetExtraPathes.Length; i < len && sprite == null; i++)
					sprite = SpriteSheetRepository.Instance.GetSpriteByName(MakeSpriteName(newValue), SpriteSheetExtraPathes[i]);
			if (sprite == null && SpriteSheetPath != string.Empty && newValue != " " && !string.IsNullOrEmpty(newValue))
			{
				//		        Debug.LogError("Can't find sprite <" + newValue + "> in " + SpriteSheetPath + " sheet");               
			}

			_spriteRenderer.sprite = sprite;

			_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.sprite != null ? 1f : 0f);
		}

	}
	
	protected virtual string MakeSpriteName(string value)
	{
		return String.Format(NguiUtils.LocalizeFormat(format), value);
	}
}
	

public class SpriteSheetRepository : IDisposable
{
	#region SINGLETON

	static SpriteSheetRepository _instance;
	public static SpriteSheetRepository Instance { 
		get {
			if (_instance == null)
				_instance = new SpriteSheetRepository ();
			return _instance;
		}
	}

	#endregion

	readonly Dictionary<string, List<Sprite>> _sheets = new Dictionary<string, List<Sprite>>();

	public Sprite GetSpriteByName(string spriteName, string spriteSheet)
	{
		if (string.IsNullOrEmpty (spriteName) || string.IsNullOrEmpty (spriteSheet))
			return null;

		Sprite sprite = null;
		if (!_sheets.ContainsKey (spriteSheet)) {
			var sprites = Resources.LoadAll<Sprite> (spriteSheet);
			if (sprites.Length > 0) {
				_sheets.Add (spriteSheet, sprites.ToList ());
				sprite = _sheets [spriteSheet].Find (s => s.name.Equals (spriteName));
			}

		} else {
			sprite = _sheets[spriteSheet].Find (s => s.name.Equals (spriteName));
		}
		return sprite;
	}

    public List<Sprite> GetAllSpriteFromSheet(string sheetName)
    {
        List<Sprite> result = new List<Sprite>();
        if (sheetName == string.Empty) return result;

        if (!_sheets.ContainsKey(sheetName))
        {
            var sprites = Resources.LoadAll<Sprite>(sheetName);
            if (sprites.Length > 0)
            {
                result = sprites.ToList();
                _sheets.Add(sheetName, result);
            }
        }
        else      
            result = _sheets[sheetName];
        
        return result;

    }

	public void Dispose ()
	{
		_sheets.Clear ();
		_instance = null;
	}

}