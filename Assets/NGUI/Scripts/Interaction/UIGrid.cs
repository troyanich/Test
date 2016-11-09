//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

/// <summary>
/// All children added to the game object with this script will be repositioned to be on a grid of specified dimensions.
/// If you want the cells to automatically set their scale based on the dimensions of their content, take a look at UITable.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	public enum Arrangement
	{
		Horizontal,
		Vertical,
	}
    public enum Sorting
    {
        None,
        Alphabetic,
        Horizontal,
        Vertical,
        Custom,
    }

	/// <summary>
	/// Type of arrangement -- vertical or horizontal.
	/// </summary>

	public Arrangement arrangement = Arrangement.Horizontal;
    public Sorting sorting = Sorting.None;
	/// <summary>
	/// Maximum children per line.
	/// If the arrangement is horizontal, this denotes the number of columns.
	/// If the arrangement is vertical, this stands for the number of rows.
	/// </summary>

	public int maxPerLine = 0;

	/// <summary>
	/// The width of each of the cells.
	/// </summary>

	public float cellWidth = 200f;

	/// <summary>
	/// The height of each of the cells.
	/// </summary>

	public float cellHeight = 200f;

	/// <summary>
	/// Whether the grid will smoothly animate its children into the correct place.
	/// </summary>

	public bool animateSmoothly = false;

	/// <summary>
	/// Whether the children will be sorted alphabetically prior to repositioning.
	/// </summary>

	public bool sorted = false;

    public bool isChat = false;

	/// <summary>
	/// Whether to ignore the disabled children or to treat them as being present.
	/// </summary>

	public bool hideInactive = true;

    public float perfectGridYPosition = 11.61f;

	/// <summary>
	/// Reposition the children on the next Update().
	/// </summary>

	public bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }

	bool mStarted = false;
	bool mReposition = false;

	Transform _cachedTr;
    void Start()
	{
		mStarted = true;
		_cachedTr = transform;
		bool smooth = animateSmoothly;
		animateSmoothly = false;
		Reposition();
		animateSmoothly = smooth;
//		enabled = false;

		if (!Loop)
			return;

		InitParameters ();

        //check GRID position
//        StartCoroutine(RecheckPosition(1f));
	}

    private IEnumerator RecheckPosition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
		if (_cachedTr.localPosition.y != perfectGridYPosition) 
        {
			_cachedTr.position = new Vector3(_cachedTr.position.x, perfectGridYPosition, _cachedTr.position.z);
        } 
		SimpleLogger.Logger.Log(_cachedTr.childCount + "position rechecked" + _cachedTr.position);
    }

	public bool Loop = false;

	public void InitParameters()
	{
		if (_cachedTr.childCount == 0)
			return;

		_posAllElements.Clear ();
		_buttonPos = new int[_cachedTr.childCount];
		for (int i = 0, imax = _cachedTr.childCount; i < imax; ++i) {
			_posAllElements.Add (0);
            _buttonPos[i] = i;
		}

        btnPosMinInList = 0;
		btnPosMaxInList = _cachedTr.childCount - 1;

        borderPosition = GetBiggestBorderPosition();
	}

	public int btnPosMinInList = 0;
    public int btnPosMaxInList = 0;
    public float borderPosition;
    public bool borderPositionSetted = false;

	readonly List<float> _posAllElements = new List<float>();
    public int[] _buttonPos;


    public void SetMinMaxPositions()
    {
        if (!borderPositionSetted)
        {
            borderPosition = GetBiggestBorderPosition();
            borderPositionSetted = true;
        } 
    }

    float GetBiggestBorderPosition() 
    {
        float x;
		int count = _cachedTr.childCount;
        if (count > 0)
        {
			x = _cachedTr.GetChild(1).position.x; // get first position border
            for (int i = 1; i < count; i++)
            {
				var t = _cachedTr.GetChild(i);
                if (Math.Abs(t.position.x) > Math.Abs(x))
                {
                    x = Math.Abs(t.position.x);
                }
            }
        }
        else {
            x = 1000; // anlimited border!!!
        }

        return x;
    }

	void Update ()
	{
        if (mReposition)
        { 
            Reposition(); 
        }

        if (!Loop)
        {
            return;
        }

        if (borderPositionSetted)
        {
			for (int i = 0, imax = _cachedTr.childCount; i < imax; i++)
            {
				var t = _cachedTr.GetChild(i);
                _posAllElements[i] = (float)Math.Round(t.position.x, 1);
                if (_posAllElements[i] > Math.Abs(borderPosition))
                {
                    this._buttonPos[i] = --btnPosMinInList;
                    --btnPosMaxInList;
                    t.localPosition = new Vector3(_buttonPos[i] * cellWidth, 0, -1);
                }

                if (_posAllElements[i] < -Math.Abs(borderPosition))
                {
                    this._buttonPos[i] = ++btnPosMaxInList;
                    ++btnPosMinInList;
                    t.localPosition = new Vector3(_buttonPos[i] * cellWidth, 0, -1);
                }
            }
        }
	}


	static public int SortByName (Transform a, Transform b) { return string.Compare(a.name, b.name); }
    static public int ReverseSortByName(Transform a, Transform b) { return string.Compare(b.name, a.name); }

    static public int SortHorizontal(Transform a, Transform b) { return a.localPosition.x.CompareTo(b.localPosition.x); }
    static public int SortVertical(Transform a, Transform b) { return b.localPosition.y.CompareTo(a.localPosition.y); }

	/// <summary>
	/// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
	/// </summary>

	[ContextMenu("Execute")]
	public void Reposition ()
	{

		if (Application.isPlaying && !mStarted) {
			mReposition = true;
			return;
		} else if(_cachedTr == null){
			_cachedTr = transform;
		}

		mReposition = false;

		int x = 0;
		int y = 0;

		if (sorted)
		{
			List<Transform> list = new List<Transform>();

			for (int i = 0; i < _cachedTr.childCount; ++i)
			{
				Transform t = _cachedTr.GetChild(i);
				if (t && (!hideInactive || NGUITools.GetActive(t.gameObject))) list.Add(t);
			}
            if (isChat)
            {
                list.Sort(ReverseSortByName);
            }
            else
            {
                list.Sort(SortByName);
            }

			for (int i = 0, imax = list.Count; i < imax; ++i)
			{
				Transform t = list[i];

				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

				float depth = t.localPosition.z;
				Vector3 pos = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);

				if (animateSmoothly && Application.isPlaying)
				{
					SpringPosition.Begin(t.gameObject, pos, 15f);
				}
				else t.localPosition = pos;

				if (++x >= maxPerLine && maxPerLine > 0)
				{
					x = 0;
					++y;
				}
			}
		}
		else
		{
			for (int i = 0; i < _cachedTr.childCount; i++)
			{
				Transform t = _cachedTr.GetChild(i);

				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;
				float depth = t.localPosition.z;
				Vector3 pos = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);

				if (animateSmoothly && Application.isPlaying)
				{
					SpringPosition.Begin(t.gameObject, pos, 15f);
				}
				else t.localPosition = pos;

                if (++x >= maxPerLine && maxPerLine > 0)
				{
					x = 0;
					y++;
				}
			}
		}

		UIScrollView drag = NGUITools.FindInParents<UIScrollView>(gameObject);
        if (drag != null) drag.UpdateScrollbars(true); 
	}
}
