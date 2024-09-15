using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Hareket ile ilgili de�i�kenler
    public float moveSpeed = 5f;  // Yatay hareket h�z�
    public float jumpForce = 5f;  // Z�plama kuvveti
    private float moveInput;      // Kullan�c� girdisi

    // Fizik ile ilgili de�i�kenler
    private Rigidbody2D rb;       // Rigidbody2D referans�
    private bool isGrounded;      // Karakterin yerde olup olmad���n� kontrol eder
    public Transform groundCheck; // Yerin kontrol edilece�i nokta
    public float checkRadius = 0.5f;  // Yerin kontrol edilece�i yar��ap
    public LayerMask whatIsGround;    // Yere neyin temas etti�ini belirlemek i�in katman

    // Animasyon ile ilgili de�i�kenler
    private Animator animator;    // Animator referans�

    // Z�plama hakk� kontrol�
    private int extraJumps;
    public int extraJumpsValue = 1;

    // Ba�lang��ta �a�r�l�r
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D bile�enini al
        animator = GetComponent<Animator>(); // Animator bile�enini al
        extraJumps = extraJumpsValue;  // Ekstra z�plamalar� ayarla
    }

    // Sabit aral�klarla fizik i�lemleri burada yap�l�r
    void FixedUpdate()
    {
        // Hareket girdisini al
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Karakterin y�n�n� ayarla
        if (moveInput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (moveInput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);

        // Yerde olup olmad���n� kontrol et
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // Animasyon parametrelerini g�ncelle
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
    }

    // Frame bazl� giri� kontrol�
    void Update()
    {
        // Yerdeyse z�plama hakk�n� yenile
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        // Z�plama girdisini kontrol et
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    // G�rsel olarak zemin kontrol alan�n� g�stermek i�in
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}