//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Ever wanted to be able to auto-center on an object within a draggable panel?
/// Attach this script to the container that has the objects to center on as its children.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	/// <summary>
	/// The strength of the spring.
	/// </summary>

	public float springStrength = 8f;

	/// <summary>
	/// If set to something above zero, it will be possible to move to the next page after dragging past the specified threshold.
	/// </summary>

	public float nextPageThreshold = 0f;

	/// <summary>
	/// Callback to be triggered when the centering operation completes.
	/// </summary>

	public SpringPanel.OnFinished onFinished;

	UIScrollView mDrag;
	GameObject mCenteredObject;
    public int flankElementsCount = 0;
    public int flankOffset = 0;
    public int pageElementsCount = 0;
	/// <summary>
	/// Game object that the draggable panel is currently centered on.
	/// </summary>

	public GameObject centeredObject { get { return mCenteredObject; } }

	void OnEnable () { Recenter(); }
	void OnDragFinished () { if (enabled) Recenter(); }

	/// <summary>
	/// Ensure that the threshold is always positive.
	/// </summary>

	void OnValidate ()
	{
		nextPageThreshold = Mathf.Abs(nextPageThreshold);
	}

	/// <summary>
	/// Recenter the draggable list on the center-most active child.
	/// </summary>
    public void Recenter()
    {
        Recenter(true);
    }
    
    /// <summary>
	/// Recenter the draggable list on the center-most child.
	/// </summary>
    public void Recenter(bool skipInactive)
	{
		if (mDrag == null)
		{
			mDrag = NGUITools.FindInParents<UIScrollView>(gameObject);

			if (mDrag == null)
			{
				Debug.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " on a parent object in order to work", this);
				enabled = false;
				return;
			}
			else
			{
				mDrag.onDragFinished = OnDragFinished;

				if (mDrag.horizontalScrollBar != null)
					mDrag.horizontalScrollBar.onDragFinished = OnDragFinished;

				if (mDrag.verticalScrollBar != null)
					mDrag.verticalScrollBar.onDragFinished = OnDragFinished;
			}
		}
		if (mDrag.panel == null) return;

		// Calculate the panel's center in world coordinates
		Vector3[] corners = mDrag.panel.worldCorners;
		Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

		// Offset this value by the momentum
		Vector3 pickingPoint = panelCenter - mDrag.currentMomentum * (mDrag.momentumAmount * 0.1f);
		mDrag.currentMomentum = Vector3.zero;

		float min = float.MaxValue;
		Transform closest = null;        
		Transform trans = transform;
		int index = 0;
        int activeIndex = 0;
        int activeCount = 0;
        Transform activeFirst = null;
        Transform activeLast = null;

		// Determine the closest child
        for (int i = 0, imax = trans.childCount - 1; i <= imax; ++i)
		{
            Transform t = trans.GetChild(i);
            if (skipInactive && !NGUITools.GetActive(t.gameObject))
                continue;

            activeCount++;
            activeLast = t;
            if (activeFirst == null)
                activeFirst = t;

            float sqrDist = Vector3.SqrMagnitude(t.position - pickingPoint);
			if (sqrDist < min)
			{
				min = sqrDist;                
                index = i;
                activeIndex = activeCount - 1;
                closest = t;                
			}                        
		}         

		// If we have a touch in progress and the next page threshold set
		if (nextPageThreshold > 0f && UICamera.currentTouch != null)
		{
			// If we're still on the same object
			if (mCenteredObject != null && mCenteredObject.transform == closest)
			{
				Vector2 delta = UICamera.currentTouch.totalDelta;

				if (delta.x > nextPageThreshold)
				{
					// Next page
					int k = index - 1;    
                    while (k >= 0) {    
                        Transform tr = trans.GetChild(k);    
                        if (skipInactive && !NGUITools.GetActive(tr.gameObject))    
                            k--;    
                        else {    
                            closest = tr;    
                            break;    
                        }    
                    }                           
				}
				else if (delta.x < -nextPageThreshold)
				{
					// Previous page
                    int k = index + 1;
                    int count = trans.childCount;
                    while (k < count) {
                        Transform tr = trans.GetChild(k);
                        if (skipInactive && !NGUITools.GetActive(tr.gameObject))
                            k++;
                        else {
                            closest = tr;
                            break;
                        }
                    }
				}
			}
		}

        if (closest != null)
        {
            if (flankElementsCount > 0)
            {
                if (activeIndex >= flankElementsCount && activeIndex <= activeCount - flankElementsCount - 1)
                {
                    CenterOn(closest, panelCenter, 0);
                }
                else
                {
                    if (activeIndex <= activeCount - activeIndex || (pageElementsCount > 0 && activeCount < pageElementsCount))
                    {
                        CenterOn(activeFirst, panelCenter, -flankOffset);
                    }
                    else
                    {
                        CenterOn(activeLast, panelCenter, flankOffset);
                    }
                }
            }
            else {
                CenterOn(closest, panelCenter, 0);
            }   

            closest.SendMessage("OnClickInTime", SendMessageOptions.DontRequireReceiver);
        }
	}

    /// <summary>
    /// Center the panel on the specified target.
    /// </summary>
    public void CenterOn(Transform target)
    {
        CenterOn(target, true);
    }    
    
    /// <summary>
    /// Center the panel on the specified target.
    /// </summary>
    public void CenterOn(Transform target, bool skipInactive)
    {
        if (mDrag != null && mDrag.panel != null)
        {
            Vector3[] corners = mDrag.panel.worldCorners;
            Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

            // Determine the child id
            Transform trans = transform;
            int count = trans.childCount;
            int activeIndex = -1;
            int activeCount = 0;
            Transform activeFirst = null;
            Transform activeLast = null;
            for (int i = 0; i < count; ++i)
            {
                Transform t = trans.GetChild(i);
                if (skipInactive && !NGUITools.GetActive(t.gameObject))
                    continue;

                activeCount++;
                activeLast = t;
                if (activeFirst == null)
                    activeFirst = t;

                if (activeIndex < 0 && t == target)
                    activeIndex = activeCount - 1;
            } 

            //center on
            if (activeCount > 0 && activeIndex >= 0 && flankElementsCount > 0)
            {
                if (activeIndex >= flankElementsCount && activeIndex <= activeCount - flankElementsCount - 1)
                {
                    CenterOn(target, panelCenter, 0);
                }
                else
                {
                    if (activeIndex <= activeCount - activeIndex || (pageElementsCount > 0 && activeCount < pageElementsCount))
                    {
                        CenterOn(activeFirst, panelCenter, -flankOffset);
                    }
                    else
                    {
                        CenterOn(activeLast, panelCenter, flankOffset);
                    }
                }
            }
            else
            {
                CenterOn(target, panelCenter, 0);
            }
        }
    }

	/// <summary>
	/// Center the panel on the specified target.
	/// </summary>
	void CenterOn (Transform target, Vector3 panelCenter, float offset)
	{
		if (target != null && mDrag != null && mDrag.panel != null)
		{
			Transform panelTrans = mDrag.panel.cachedTransform;
			mCenteredObject = target.gameObject;

            // Figure out the available offset
            Vector3 offset3D = Vector3.zero;
            if (mDrag.canMoveHorizontally && !mDrag.canMoveVertically)
                offset3D = Vector3.right * offset;
            else
            if (mDrag.canMoveVertically && !mDrag.canMoveHorizontally)
                offset3D = Vector3.up * offset;

			// Figure out the difference between the chosen child and the panel's center in local coordinates
            Vector3 cp = panelTrans.InverseTransformPoint(target.TransformPoint(offset3D));
			Vector3 cc = panelTrans.InverseTransformPoint(panelCenter);
			Vector3 localOffset = cp - cc;

			// Offset shouldn't occur if blocked
			if (!mDrag.canMoveHorizontally) localOffset.x = 0f;
			if (!mDrag.canMoveVertically) localOffset.y = 0f;
			localOffset.z = 0f;

			// Spring the panel to this calculated position
			SpringPanel.Begin(mDrag.panel.cachedGameObject,
				panelTrans.localPosition - localOffset, springStrength).onFinished = onFinished;
		}
		else mCenteredObject = null;
	}
	
}
