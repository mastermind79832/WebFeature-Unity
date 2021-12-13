using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float jump;

    private float xInput;

    private bool onWeb = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
      
        if(Input.GetAxis("Horizontal") != 0)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity = new Vector2(speed * xInput, rb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }

    }


}
