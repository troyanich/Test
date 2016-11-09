using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UniRx;

public class ClickController : MonoBehaviour 
{

	NavMeshAgent _dude;

	void Awake ()
	{
		_dude = GetComponent<NavMeshAgent> ();
	}

	RaycastHit _hitInfo = new RaycastHit ();
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray.origin, ray.direction, out _hitInfo)) {
				Debug.Log ("Click " + _hitInfo.point);
				_dude.SetDestination (_hitInfo.point);
			}
		}
#endif

#if !UNITY_EDITOR && UNITY_IOS

		if (!EventSystem.current.IsPointerOverGameObject () && Input.touchCount > 0) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray.origin, ray.direction, out _hitInfo)) {
				Dude.SetDestination (_hitInfo.point);
			}
		}

#endif


	}
}
