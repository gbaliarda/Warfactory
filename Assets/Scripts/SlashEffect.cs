using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 2f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private bool _fadeOut = true;
    
    private SpriteRenderer _spriteRenderer;
    private bool _facingRight;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // Determine facing direction based on scale
        _facingRight = transform.localScale.x > 0;
    }

    private void Update()
    {
        if (_fadeOut)
        {
            // Fade out
            Color color = _spriteRenderer.color;
            color.a -= _fadeSpeed * Time.deltaTime;
            _spriteRenderer.color = color;

            // Move in the correct direction based on facing
            Vector3 moveDirection = _facingRight ? Vector3.right : Vector3.left;
            transform.Translate( _moveSpeed * Time.deltaTime * moveDirection);

            if (color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}