using System;
using System.Collections;

using UnityEngine;


public class PlayerMover : MonoBehaviour
{
	public float speed = 3.0f;
	public float rotateSpeed = 3.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	
	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	private bool grounded = false;
	
	// Use this for initialization
	void Start()
	{
		controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update()
	{
		Transform camTransf = Camera.main.transform;
		Vector3 dir = Vector3.zero;
			
		if (Input.GetKey(KeyCode.W))
		{
			moveDirection = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			moveDirection = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * -1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			moveDirection = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			moveDirection = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized * -1;
		}
		else
		{
			moveDirection = new Vector3(0, moveDirection.y, 0);
		}
		
		
		// Move the controller
		CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime * speed);
		grounded = (flags & CollisionFlags.CollidedBelow) != 0;
			
		if ((moveDirection.x != 0.0f || moveDirection.x != 0.0f))
		{
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,
			   							      camTransf.rotation.eulerAngles.y,
										      transform.rotation.eulerAngles.z));
			
			gameObject.GetComponent<Animation>().Play("run");
		}
		else
		{
			Quaternion rotTo = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,
											  camTransf.rotation.eulerAngles.y,
										      transform.rotation.eulerAngles.z));
		
			transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, Time.deltaTime);
			
			gameObject.GetComponent<Animation>().Play("idle");
		}
	}
}
