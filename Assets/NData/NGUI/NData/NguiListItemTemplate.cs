using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
[AddComponentMenu("NGUI/NData/List Item Template")]
public class NguiListItemTemplate : MonoBehaviour
{
	public GameObject Template;
	public UIDraggableCamera DraggableCamera;

	public GameObject Instantiate(EZData.Context itemContext, int index, int groupNumber)
	{
		if (Template == null)
		{
			return null;
		}

		GameObject instance = Instantiate (Template);


		
		var subTemplates = instance.GetComponentsInChildren<NguiListItemTemplate>();
		foreach (var st in subTemplates)
		{
			if (st.Template == instance)
				st.Template = Template;
		}
		
		foreach(UIDragCamera cd in instance.GetComponentsInChildren<UIDragCamera>())
		{
			if (cd.draggableCamera == null && DraggableCamera != null)
				cd.draggableCamera = DraggableCamera;
		}
		var itemData = instance.AddComponent<NguiItemDataContext>();

        instance.transform.SetParent(transform);
		itemData.SetContext(itemContext);
		instance.transform.SetParent(null);
		
		itemData.SetIndex(index);
			
		var toggle = itemData.GetComponent<UIToggle> ();

		if (toggle != null) {
			toggle.group = groupNumber;
		}

//		if(retinaProUtil.sharedInstance != null)
//			retinaProUtil.sharedInstance.refreshVisible (instance);

        return instance;
	}
}
