using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player;

	private Vector3 offset;

	void Start()
	{
		offset = transform.position - player.transform.position;
	}

	void LateUpdate()
	{
		transform.position = player.transform.position + offset;
	}
}


/*
public Transform target;

private Vector3 offsetPosition;

private Space offsetPositionSpace = Space.Self;

private bool lookAt = true;


void Start () {
	offsetPosition = transform.position - target.position; 
}

void LateUpdate()
{
	Refresh();
}

public void Refresh()
{
	if(target == null)
	{
		Debug.LogWarning("Missing target ref !", this);

		return;
	}

	// compute position
	if(offsetPositionSpace == Space.Self)
	{
		transform.position = target.TransformPoint(offsetPosition);
	}
	else
	{
		transform.position = target.position + offsetPosition;
	}

	// compute rotation
	if(lookAt)
	{
		transform.LookAt(target);
	}
	else
	{
		transform.rotation = target.rotation;
	}
}
*/