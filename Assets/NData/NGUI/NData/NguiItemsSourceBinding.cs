using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
[AddComponentMenu("NGUI/NData/ItemsSource Binding")]
public class NguiItemsSourceBinding : NguiBinding
{
    public bool AddToHead = false;
    
    protected NguiListItemTemplate _itemTemplate;
    protected EZData.Collection _collection;
    protected bool _isCollectionSelecting = false;

    private UITable _uiTable = null;
    private UIGrid _uiGrid = null;
	protected ScrollRect _scrollRect;

    int _groupNumber;
    readonly List<int> _usedGroupNumbers = new List<int>();

    private Dictionary<GameObject, NguiItemDataContext> _cachedChilds = new Dictionary<GameObject, NguiItemDataContext>();

    private readonly List<GameObject> _cache = new List<GameObject>(16);
    private readonly List<NguiItemDataContext> _contexts = new List<NguiItemDataContext>();
//    FooterGridLayoutGroup footer;
    public override void Awake()
    {
        base.Awake();
        _groupNumber = Random.Range(1, 10000);
        if (_usedGroupNumbers.Contains(_groupNumber))
            _groupNumber++;
        _usedGroupNumbers.Add(_groupNumber);
        _uiTable = GetComponent<UITable>();
        _uiGrid = GetComponent<UIGrid>();
        _itemTemplate = gameObject.GetComponent<NguiListItemTemplate>();
		if (transform.parent != null) { 
			_scrollRect = transform.parent.GetComponent<ScrollRect> ();
		}
//        if(_scrollRect)
//			GridController = _scrollRect.transform.parent.GetComponent<LeaderboardsGridController>();
//        footer = gameObject.GetComponent<FooterGridLayoutGroup>();
    }

    protected override void Unbind()
    {
        base.Unbind();

        if (_collection != null)
        {
            _collection.OnItemInsert -= OnItemInsert;
            _collection.OnItemRemove -= OnItemRemove;
            _collection.OnItemsClear -= OnItemsClear;
            _collection.OnSelectionChange -= OnCollectionSelectionChange;
            _collection = null;
            OnItemsClear();
        }
    }

    protected override void Bind()
    {
        base.Bind();

        var context = GetContext(Path);
        if (context == null)
            return;

        _collection = context.FindCollection(Path, this);
        if (_collection == null)
            return;

        _collection.OnItemInsert += OnItemInsert;
        _collection.OnItemRemove += OnItemRemove;
        _collection.OnItemsClear += OnItemsClear;
        _collection.OnSelectionChange += OnCollectionSelectionChange;

        for (var i = 0; i < _collection.ItemsCount; ++i)
        {
            OnItemInsert(i, _collection.GetBaseItem(i));
        }
        OnCollectionSelectionChange();
    }
    public int MaxItemCount = 1000;
    protected virtual void OnItemInsert(int position, EZData.Context item)
    {
        GameObject itemObject = null;
        if (_itemTemplate != null && _cache.Count < MaxItemCount)
        {
            itemObject = _itemTemplate.Instantiate(item, position, _groupNumber);

            _cache.Insert(position, itemObject);

            itemObject.transform.SetParent(transform);
            if (AddToHead)
                itemObject.transform.SetAsFirstSibling();
            itemObject.transform.localScale = Vector3.one;
            itemObject.transform.localPosition = Vector3.back;

            //TODO: IMPORTANT
//            retinaProUtil.sharedInstance.refreshVisible(itemObject);

        }
        else
        {
            if (position < transform.childCount)
            {
                itemObject = transform.GetChild((!AddToHead) ? position : (transform.childCount - 1 - position)).gameObject;
                var itemData = itemObject.GetComponent<NguiItemDataContext>();
                if (itemData != null)
                {
                    itemData.SetContext(item);
                    itemData.SetIndex(position);
                }
            }
        }
        if (itemObject != null)
        {
            foreach (var dragObject in itemObject.GetComponentsInChildren<UIDragObject>())
            {
                if (dragObject.target == null)
                    dragObject.target = transform;
            }
            foreach (var dragObject in itemObject.GetComponents<UIDragObject>())
            {
                if (dragObject.target == null)
                    dragObject.target = transform;
            }

            var parentVisibility = NguiUtils.GetComponentInParentsAs<IVisibilityBinding>(gameObject);
            foreach (var visibility in NguiUtils.GetComponentsInChildrenAs<IVisibilityBinding>(itemObject))
            {
                visibility.InvalidateParent();
            }
            var visible = parentVisibility == null ? true : parentVisibility.Visible;
            NguiUtils.SetVisible(itemObject, visible);

            RepositionContent();
        }
    }

    protected virtual void OnItemRemove(int position)
    {
        if (_itemTemplate == null)
            return;

        var item = _cache[position];
        _cache.RemoveAt(position);
        DestroyImmediate(item);

        RepositionContent();
    }

    private void RepositionContent()
    {
        if (_uiTable != null)
        {
            _uiTable.repositionNow = true;
        }

        if (_uiGrid != null)
        {
            var parentLookup = NguiUtils.GetComponentInParentsExcluding<UITable>(
                gameObject);
            if (parentLookup == null)
                _uiGrid.repositionNow = true;
            else
                _uiGrid.Reposition();
        }

        if (_scrollRect != null)
        {
            CancelInvoke("NormalizeScrollRect");
            Invoke("NormalizeScrollRect", 1f);
        }
    }
//	protected LeaderboardsGridController GridController { private set; get; }
	protected virtual void NormalizeScrollRect()
    {
//        print("scrollRect reposition");
        //scrollRect.normalizedPosition = new Vector2(0f, 1f);
//		if (GridController && transform.childCount > 0)
//        {
//			GridController.Init();
//        }

    }
    protected virtual void OnItemsClear()
    {

        if (_itemTemplate == null)
            return;

        foreach (var item in _cache)
        {
            DestroyImmediate(item);
        }
        _cache.Clear();

        RepositionContent();
    }

    public virtual void OnSelectionChange(GameObject selectedObject)
    {
        if (_collection != null && !_isCollectionSelecting)
        {
            _isCollectionSelecting = true;

            for (int i = 0; i < _cache.Count; i++)
            {
                var child = _cache[i];
                if (child == selectedObject)
                {
                    _collection.SelectItem(i);
                    break;
                }
            }

            _isCollectionSelecting = false;
        }
    }

    protected virtual void OnCollectionSelectionChange()
    {
        for (int i = 0; i < _cache.Count; i++)
        {
            var child = _cache[i];
            var itemData = child.GetComponent<NguiItemDataContext>();
            if (itemData != null)
                itemData.SetSelected(i == _collection.SelectedIndex);
        }
    }

    PointerEventData pointerData;
    public void OnDrag()
    {
//        PointerEventData pointerData = data as PointerEventData;
        int size = _collection.ItemsCount;
        if (size < MaxItemCount)
            return;
		/*
            while (_cache[MaxItemCount - 1].GetComponent<NguiItemDataContext>().Index < (size - 1))
            {
                if (footer.Change(_cache[0].transform, true))
                {
                    GameObject temp = _cache[0];
                    _cache.RemoveAt(0);
                    NguiItemDataContext context = temp.GetComponent<NguiItemDataContext>();
                    context.SetIndex(context.Index + MaxItemCount);
                    context.SetContext(_collection.GetBaseItem(context.Index));
                    _cache.Add(temp);
                }
                else
                    break;
            }

            while (_cache[0].GetComponent<NguiItemDataContext>().Index > 0)
            {
                if (footer.Change(_cache[MaxItemCount - 1].transform, false))
                {
                    GameObject temp = _cache[MaxItemCount - 1];
                    _cache.RemoveAt(MaxItemCount - 1);
                    NguiItemDataContext context = temp.GetComponent<NguiItemDataContext>();
                    context.SetIndex(context.Index - MaxItemCount);
                    context.SetContext(_collection.GetBaseItem(context.Index));
                    _cache.Insert(0, temp);
                }
                else
                    break;
            }
            */
    }
}
