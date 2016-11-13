using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public GameObject game;
    public int health = 3;
    public Text healthLabel;

    private GameController gameController;
	private Rigidbody rb;
    private Animator animator;

    void Start () {
        rb = GetComponent<Rigidbody> ();
        gameController = game.GetComponent<GameController>();
        printHealthLabel();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per fram;
	void FixedUpdate () {
		if (!gameController.isPaused())
		{
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, -z);

            if (x != 0 || z != 0)
            {
                animator.SetBool("Charge", true);
                animator.SetBool("Idle", false);
            } else
            {
                animator.SetBool("Charge", false);
                animator.SetBool("Idle", true);
            }
        }
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Goal"))
		{
			gameController.win ();
		}
		else if (other.gameObject.CompareTag ("Trap"))
		{
			reduceHealth ();

		}
	}

	// This can be added with a timer. If not is called in almost all the frames
	void OnTriggerStay(Collider other)
	{
	}

	void reduceHealth()
	{
		health--;
        printHealthLabel();
        if (health == 0) {
			gameController.lose ();
		}
	}

    private void printHealthLabel()
    {
        healthLabel.text = string.Format("Health: {0}", health);
    }
}
