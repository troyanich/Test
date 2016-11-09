using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

	public float Speed;
	//public Transform Target;
	public Vector3 Offset;
	Vector3 _targetPos;

	Transform _cachedTr;

	void Awake ()
	{
		_cachedTr = transform;
	}

	Vector3 _velocity;

	// Update is called once per frame
	void Update ()
	{
	    if (GameManager.instance == null || GameManager.instance.Character == null)
	        return;

	    _targetPos = GameManager.instance.Character.transform.position; //Target.position;
        //print(_targetPos);
	    //_targetPos.y = _cachedTr.position.y;

		_cachedTr.position = Vector3.SmoothDamp (_cachedTr.position, _targetPos + Offset, ref _velocity, Speed);
	}
}

