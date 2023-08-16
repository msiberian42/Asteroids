using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private float fireIncreasePerSecond;
    [SerializeField] private float fireDecreaseByExtinguish;

    public static bool isOnFire { get; private set; }
    public static float fireValue { get; private set; }
    private int maxFireValue = 100;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        fireValue = 1;
    }
    private void Update()
    {
        if (fireValue <= 0)
        {
            fireValue = 1;
            isOnFire = false;
        }
        if (fireValue >= maxFireValue)
        {
            fireValue = maxFireValue;
            isOnFire = false;
            player.KillPlayer();
        }

        if (isOnFire) BurnPlayer();
    }
    public void BurnPlayer()
    {
        fireValue += fireIncreasePerSecond * Time.deltaTime;
    }
    private void Extinguish()
    {
        fireValue -= fireDecreaseByExtinguish;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exting"))
        {
            Extinguish();
        }
        if (collision.CompareTag("Asteroid"))
        {
            isOnFire = true;
        }
    }

}