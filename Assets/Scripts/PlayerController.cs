using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int count;
    private float startTime;
    private bool isTimerRunning = true;
    private float timeRemaining = 10f;
    private int pickupsCollected = 0;
    private AudioSource audioSource;
    public TextMeshProUGUI timerText;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public Transform respawnPoint;
    public MenuController menuController;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        startTime = Time.time;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            Respawn();
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            EndGame();
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if (isTimerRunning)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            timerText.text = "Timer: " + seconds;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            pickupsCollected++;
            timeRemaining += 2f; // Aumenta 2 segundos
            UpdateTimerText(); 

            if (pickupsCollected % 12 == 0)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
                newEnemy.GetComponent<Enemy>().SetPlayer(transform); // Define a referência do jogador
                speed+= 0.15f;
                Enemy.speedIncrease += 0.05f;
            }

            audioSource.Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EndGame();
        }
    }

    void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        transform.position = respawnPoint.position;
    }

    void EndGame()
    {
        menuController.LoseGame();
        gameObject.SetActive(false);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 108)
        {
            menuController.WinGame();
            isTimerRunning = false;
        }
    }

    void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.RoundToInt(timeRemaining).ToString();
    }
}
