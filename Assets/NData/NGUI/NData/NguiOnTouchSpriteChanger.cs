using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnTouchSpriteChanger Binding")]
public class NguiOnTouchSpriteChanger : NguiCommandBinding
{
    UISprite sprite;
    UILabel label;
    string normalSprite;
    string pressedSprite;

    private const string TEXT_COLOR_NORMAL = "[544234]";
    private const string TEXT_COLOR_PRESSED = "[f5b247]";

    private const string SPRITE_SUFIX_NORMAL = "_normal";
    private const string SPRITE_SUFIX_PRESSED = "_pressed";

    void Awake() 
    {
		var tr = transform.FindChild ("Icon");
		if (tr == null) {
			tr = transform.GetChild (0).FindChild ("Icon");
		}
		sprite = tr.GetComponent<UISprite>();
        var name = sprite.spriteName.Substring(0, sprite.spriteName.LastIndexOf('_'));
        normalSprite = name + SPRITE_SUFIX_NORMAL;
        pressedSprite = name + SPRITE_SUFIX_PRESSED;


		tr = transform.FindChild ("txt_AgencyFB_36");
		if (tr == null) {
			tr = transform.GetChild (0).FindChild ("txt_AgencyFB_36");
		}
        label = tr.GetComponent<UILabel>();
    }

    void OnPress(bool pressed)
    {
        if (pressed)
        {
            sprite.spriteName = pressedSprite;
            if (label.text[0].Equals('['))
            {
                label.text = TEXT_COLOR_PRESSED + label.text.Split(']')[1];
            }
            else 
            {
                label.text = TEXT_COLOR_PRESSED + label.text;
            }
        }
        else
        {
            sprite.spriteName = normalSprite;
            if (label.text[0].Equals('['))
            {
                label.text = TEXT_COLOR_NORMAL + label.text.Split(']')[1];
            }
            else
            {
                label.text = TEXT_COLOR_NORMAL + label.text;
            }
        }
    }

    void UpdateImage()
    {
        if (sprite != null)
        {
            sprite.MakePixelPerfect();
        }
    }
}
