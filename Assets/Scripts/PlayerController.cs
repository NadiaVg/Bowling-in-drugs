using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed = 5;

    private Rigidbody rb;

    private float movementX;
    private float movementY;

    public float jumpForce;
	public bool canJump = true;

    private Vector3 spawnpoint;

    private GameObject[] pins;

    private int count;
    public TextMeshProUGUI countText;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnpoint = GetComponent<Transform>().position;
        pins = GameObject.FindGameObjectsWithTag("Pin");
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

        if(Input.GetKey("space") && canJump)
        {
			rb.AddForce(new Vector3(0, 1 * jumpForce, 0));
			canJump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
			canJump = true;
        }
		if (collision.gameObject.CompareTag("OffBorders"))
		{
			Transform reset = GetComponent<Transform>();
			reset.position = spawnpoint;
			rb.velocity = new Vector3(0, 0, 0);
		}
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PickUp"))
        {
                    other.gameObject.SetActive(false);
           
            Transform transform = GetComponent<Transform>();
            transform.localScale = new Vector3(transform.localScale.x + 0.5f, transform.localScale.z + 0.5f, transform.localScale.y + 0.5f);

            // Run the 'SetCountText()' function (see below)
           

        }

        if (other.gameObject.CompareTag("Goal"))
        {
            StartCoroutine(calculatedPoints());
           
            
        }
    }

    IEnumerator calculatedPoints()
    {
        yield return new WaitForSeconds(2f);

        foreach (GameObject pin in pins)
        {
            Debug.Log(pin.name + " - " + pin.GetComponent<Transform>().localRotation);
            if (pin.GetComponent<Transform>().localRotation != new Quaternion(0.00000f, 0.00000f, 0.00001f, 1.00000f))
            {
                count++;
            }
        }
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Pins: " + count.ToString();

        StartCoroutine(Next());
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
