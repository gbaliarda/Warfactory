using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int LookDirXParam = Animator.StringToHash("LookDirX");
    private static readonly int MovingParam = Animator.StringToHash("Moving");
    private static readonly int WalkSpeedMultiplierParam = Animator.StringToHash("WalkSpeedMultiplier");

    [SerializeField]
    private float _movingVelocityCutoff = 0.01f;

    [SerializeField] private Transform _hotbarItems;
    [SerializeField] private Transform _buildHotbarItems;
    [SerializeField] private float _animationMovementSpeed = 1f;

    private Animator _animator;
    private Transform _currentHotbarItems;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentHotbarItems = _hotbarItems;
    }

    private void Start()
    {
        EventManager.Instance.OnBuildModeActive += OnBuildModeActive;
    }

    private void OnBuildModeActive(bool active)
    {
        if (active) _currentHotbarItems = _buildHotbarItems;
        else _currentHotbarItems = _hotbarItems;
    }

    private void LateUpdate()
    {
        UpdateLookDir();
        UpdateMoving();
    }

    private void UpdateMoving()
    {
        var vel = Player.Instance.Rigidbody.velocity;
        _animator.SetBool(MovingParam, vel.sqrMagnitude > _movingVelocityCutoff);
        _animator.SetFloat(WalkSpeedMultiplierParam, vel.magnitude / _animationMovementSpeed);
    }

    private void UpdateLookDir()
    {
        var cam = Camera.main;
        if(!cam) return;

        var mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (transform.position - mousePosition).normalized;

        var simplifiedDir = Vector2.zero;
        simplifiedDir.y = 0;
        simplifiedDir.x = dir.x > 0 ? -1 : 1;

        _animator.SetInteger(LookDirXParam, (int)simplifiedDir.x);

        UpdateHotbarItemRotation(simplifiedDir);
    }

    private void UpdateHotbarItemRotation(Vector2 lookDir)
    {
        if (lookDir.x > 0)
        {
            _currentHotbarItems.right = transform.right;
            _currentHotbarItems.localPosition = Vector3.zero;
            SortHotbarItemsBeforePlayer(false);
            FlipHotbarItems(false, false);
        } else if (lookDir.x < 0)
        {
            _currentHotbarItems.right = -transform.right;
            _currentHotbarItems.localPosition = Vector3.zero;
            SortHotbarItemsBeforePlayer(false);
            FlipHotbarItems(false, false);
        }
    }

    private void FlipHotbarItems(bool flipX, bool flipY)
    {
        var sprites = _currentHotbarItems.GetComponentsInChildren<SpriteRenderer>();


        foreach (var sprite in sprites)
        {
            sprite.flipX = flipX;
            sprite.flipY = flipY;
        }
    }

    private void SortHotbarItemsBeforePlayer(bool before)
    {
        var sprites = _currentHotbarItems.GetComponentsInChildren<SpriteRenderer>();

        foreach (var sprite in sprites)
        {
            sprite.sortingOrder = before ? 99 : 101;
        }
    }
}
