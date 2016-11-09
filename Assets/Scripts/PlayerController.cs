using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public KeyCode forward;
	public KeyCode right;
	public KeyCode left;

	public float speed;
	public float turnSpeed;
	public int life;

	private GameController gameController;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		gameController = GetComponent<GameController> ();
	}
	
	// Update is called once per fram;
	void FixedUpdate () {
		if (!gameController.isPaused())
		{
			Vector3 vel = transform.forward * speed;

			if (Input.GetKey(left))
			{
				transform.Rotate (-Vector3.up * turnSpeed * Time.deltaTime);
			} 
			if (Input.GetKey(right))
			{
				transform.Rotate (Vector3.up * turnSpeed  * Time.deltaTime);
			}
			if (Input.GetKey (forward)) {
				transform.position -= transform.forward * Time.deltaTime * speed;
			} 
			else {
				rb.velocity = new Vector3 (0, 0, 0);
			}
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Goal"))
		{
			gameController.win ();
		}
	}
}
