using UnityEngine;
using System.Collections;

public class GreatAxeScript : MonoBehaviour {

    public Vector3 direction;
    public float speed = 3;

	void Update () {
        transform.Rotate(direction * speed);
	}
}
