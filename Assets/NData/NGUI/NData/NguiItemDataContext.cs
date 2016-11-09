using EZData;
using UnityEngine;

[System.Serializable]
public class NguiItemDataContext : NguiDataContext
{
    private NguiItemsSourceBinding _cachedItemsSource;
    protected NguiItemsSourceBinding ItemsSource
    {
        get
        {
            if (_cachedItemsSource == null)
                _cachedItemsSource = NguiUtils.GetComponentInParents<NguiItemsSourceBinding>(gameObject);
            return _cachedItemsSource;
        }
    }

    public event System.Action OnSelectedChange;

    CenterOnObject _centerOnObject;
    UIToggle _toggle;
	UnityEngine.UI.Button _button;

    public bool _firstStart = true;

    public T GetContext<T>() where T : Context
    {
        return (T)_context;
    }

    protected virtual void Awake()
    {
        _centerOnObject = GetComponent<CenterOnObject>();
        _toggle = GetComponent<UIToggle>();
		_button = GetComponent<UnityEngine.UI.Button> ();


		if (_button != null) {
			_button.onClick.AddListener (OnClick);
		}
    }

    private bool _selected;
    public bool Selected
    {
        get { return _selected; }
        private set
        {

            bool needUpdate = (value != _selected) && (OnSelectedChange != null);
            _selected = value;

            if (_toggle != null && _selected)
            {
                _toggle.value = _selected;
            }

            if (_centerOnObject != null && _selected)
            {
                _centerOnObject.OnClick();
            }

            if (needUpdate)
            {
                OnSelectedChange();

            }
        }
    }
    public int Index { get; private set; }

    float _currentTime;

    protected virtual void FixedUpdate()
    {
        if (!_firstStart)
            return;

        _currentTime += Time.deltaTime;

        if (_currentTime >= 0.5f && _centerOnObject != null && _selected)
        {
            _centerOnObject.OnClick();
            _firstStart = false;
        }

    }

    protected virtual void OnClick()
    {
        if (ItemsSource != null)
            ItemsSource.OnSelectionChange(gameObject);
    }

    protected virtual void OnPress(bool pressed)
    {
    }

    protected virtual void OnDrag(Vector2 delta)
    {
    }

    public void SetSelected(bool selected)
    {
        Selected = selected;
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public void SetContext(EZData.Context c)
    {
        _context = c;
        if(gameObject.name == "FixedPlayerItem")
            print("___");
            
        var bindings = gameObject.GetComponentsInChildren<NguiBinding>();
        foreach (var binding in bindings)
        {
            binding.UpdateBinding();
        }

        var multiBindings = gameObject.GetComponentsInChildren<NguiMultiBinding>();
        foreach (var binding in multiBindings)
        {
            binding.UpdateBinding();
        }

        c.BindedGameObject = gameObject;
    }
}
