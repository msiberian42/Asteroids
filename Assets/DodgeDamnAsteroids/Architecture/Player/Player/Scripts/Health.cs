using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

namespace Gameplay
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _startHealthValue = 3;
        [SerializeField] private int _maxHealthValue = 6;
        [SerializeField] private float invincibilityTime;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private float blinkRate;
        public static int startHealthValue { get; private set; }
        public static int maxHealthValue { get; private set; }
        public static int healthValue { get; private set; }
        public static bool isInvincible { get; private set; }

        private Player player;
        private string heartTag = TagStorage.heartTag;
        private string asterTag = TagStorage.asterTag;
        private string projectileTag = TagStorage.UFOProjectileTag;

        private float reducedAlpha;
        private float normalAlpha;
        private float alphaCoeff = 0.25f;
        private float timer = 0f;

        private void Awake()
        {
            player = GetComponent<Player>();

            normalAlpha = sprite.color.a;
            reducedAlpha = normalAlpha * alphaCoeff;

            startHealthValue = _startHealthValue;
            maxHealthValue = _maxHealthValue;
            healthValue = startHealthValue;
        }
        private void Update()
        {
            if (isInvincible)
            {
                Blink();
                SetInvincibilityTimer();
            }
            else
                ReturnToNormalAlpha();
        }
        public void IncreaseHealth()
        {
            if (healthValue < maxHealthValue)
            healthValue++;
        }
        public void DecreaseHealth()
        {
            if (isInvincible) return;

            if (healthValue > 0)
            {
                healthValue--;
                isInvincible = true;
            }
            else
                return;

            if (healthValue <= 0)
                player.KillPlayer();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(heartTag))
                IncreaseHealth();
            
            if (collision.CompareTag(asterTag) || collision.CompareTag(projectileTag))
                DecreaseHealth();
        }
        private void Blink()
        {
            if (sprite.color.a == normalAlpha)
                sprite.DOFade(reducedAlpha, blinkRate);

            if (sprite.color.a == reducedAlpha)
                sprite.DOFade(normalAlpha, blinkRate);
        }
        private void SetInvincibilityTimer()
        {
            if (timer >= invincibilityTime)
            {
                isInvincible = false;
                timer -= invincibilityTime;
            }
            else
                timer += Time.deltaTime;
        }
        private void ReturnToNormalAlpha()
        {
            Color color = sprite.color;
            color.a = normalAlpha;
            sprite.color = color;
        }
    }
}
