using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 5f;
    bool isFacingRight = false;
    float jumpPower = 4f;
    bool isJumping = false;

    Rigidbody2D rb;
    AudioSource audioSource;

    private int coinCounter = 0;
    public TMP_Text counterText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0f)
        {
            Debug.Log("Input ontvangen: " + horizontalInput);
        }

        FlipSprite();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(rb.linearVelocity.x) > 0.01f)
        {
            Debug.Log("Speler beweegt. Velocity = " + rb.linearVelocity);
        }
    }

    void FlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isJumping = false;
        }
        else if (collision.gameObject.CompareTag("Coin") && collision.gameObject.activeSelf)
        {
            audioSource.Play();
            collision.gameObject.SetActive(false);
            coinCounter += 1;
            counterText.text = "Coins: " + coinCounter;
        }
    }
}
