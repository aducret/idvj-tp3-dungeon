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
    private bool jumping = false;
    private float jumpTimer = 0;

    void Start () {
        rb = GetComponent<Rigidbody> ();
        // gameController = game.GetComponent<GameController>();
        gameController = GameObject.FindGameObjectWithTag("Game").GetComponent<GameController>();
        healthLabel = GameObject.FindGameObjectWithTag("HealthLabel").GetComponent<Text>();
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

            if (jumpTimer >= 1f)
            {
                jumping = false;
            }

            if (jumping)
            {
                jumpTimer += Time.deltaTime;
            }

            if (x != 0 || z != 0)
            {
                animator.SetBool("Charge", true);
                animator.SetBool("Idle", false);
            } else
            {
                animator.SetBool("Charge", false);
                animator.SetBool("Idle", true);
            }

            if (Input.GetAxis("Jump") != 0 && !jumping)
            {
                var y = Input.GetAxis("Jump") * Time.deltaTime * 20000f;
                rb.AddForce(new Vector3(0, y, 0), ForceMode.Impulse);
                jumping = true;
                jumpTimer = 0;
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
