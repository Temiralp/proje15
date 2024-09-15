using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Hareket ile ilgili deðiþkenler
    public float moveSpeed = 5f;  // Yatay hareket hýzý
    public float jumpForce = 5f;  // Zýplama kuvveti
    private float moveInput;      // Kullanýcý girdisi

    // Fizik ile ilgili deðiþkenler
    private Rigidbody2D rb;       // Rigidbody2D referansý
    private bool isGrounded;      // Karakterin yerde olup olmadýðýný kontrol eder
    public Transform groundCheck; // Yerin kontrol edileceði nokta
    public float checkRadius = 0.5f;  // Yerin kontrol edileceði yarýçap
    public LayerMask whatIsGround;    // Yere neyin temas ettiðini belirlemek için katman

    // Animasyon ile ilgili deðiþkenler
    private Animator animator;    // Animator referansý

    // Zýplama hakký kontrolü
    private int extraJumps;
    public int extraJumpsValue = 1;

    // Baþlangýçta çaðrýlýr
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D bileþenini al
        animator = GetComponent<Animator>(); // Animator bileþenini al
        extraJumps = extraJumpsValue;  // Ekstra zýplamalarý ayarla
    }

    // Sabit aralýklarla fizik iþlemleri burada yapýlýr
    void FixedUpdate()
    {
        // Hareket girdisini al
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Karakterin yönünü ayarla
        if (moveInput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (moveInput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);

        // Yerde olup olmadýðýný kontrol et
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // Animasyon parametrelerini güncelle
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
    }

    // Frame bazlý giriþ kontrolü
    void Update()
    {
        // Yerdeyse zýplama hakkýný yenile
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        // Zýplama girdisini kontrol et
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

    // Görsel olarak zemin kontrol alanýný göstermek için
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}