using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int LookDirXParam = Animator.StringToHash("LookDirX");
    private static readonly int LookDirYParam = Animator.StringToHash("LookDirY");
    private static readonly int MovingParam = Animator.StringToHash("Moving");
    private static readonly int WalkSpeedMultiplierParam = Animator.StringToHash("WalkSpeedMultiplier");

    [SerializeField]
    private float _movingVelocityCutoff = 0.01f;

    [SerializeField] private Transform _hotbarItems;
    [SerializeField] private Vector3 _hotbarUpOffset;
    [SerializeField] private Vector3 _hotbarDownOffset;
    [SerializeField] private float _animationMovementSpeed = 1f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            simplifiedDir.y = 0;
            simplifiedDir.x = dir.x > 0 ? -1 : 1;
        }
        else
        {
            simplifiedDir.x = 0;
            simplifiedDir.y = dir.y > 0 ? -1 : 1;
        }

        _animator.SetInteger(LookDirXParam, (int)simplifiedDir.x);
        _animator.SetInteger(LookDirYParam, (int)simplifiedDir.y);

        UpdateHotbarItemRotation(simplifiedDir);
    }

    private void UpdateHotbarItemRotation(Vector2 lookDir)
    {
        if (lookDir.x > 0)
        {
            _hotbarItems.right = transform.right;
            _hotbarItems.localPosition = Vector3.zero;
            SortHotbarItemsBeforePlayer(false);
            FlipHotbarItems(false, false);
        } else if (lookDir.x < 0)
        {
            _hotbarItems.right = -transform.right;
            _hotbarItems.localPosition = Vector3.zero;
            SortHotbarItemsBeforePlayer(false);
            FlipHotbarItems(false, false);
        } else if(lookDir.y > 0)
        {
            _hotbarItems.right = transform.up;
            _hotbarItems.localPosition = _hotbarUpOffset;
            SortHotbarItemsBeforePlayer(true);
            FlipHotbarItems(false, true);
        } else if(lookDir.y < 0)
        {
            _hotbarItems.right = -transform.up;
            _hotbarItems.localPosition = _hotbarDownOffset;
            SortHotbarItemsBeforePlayer(false);
            FlipHotbarItems(false, true);
        }
    }

    private void FlipHotbarItems(bool flipX, bool flipY)
    {
        var sprites = _hotbarItems.GetComponentsInChildren<SpriteRenderer>();

        foreach (var sprite in sprites)
        {
            sprite.flipX = flipX;
            sprite.flipY = flipY;
        }
    }

    private void SortHotbarItemsBeforePlayer(bool before)
    {
        var sprites = _hotbarItems.GetComponentsInChildren<SpriteRenderer>();

        foreach (var sprite in sprites)
        {
            sprite.sortingOrder = before ? 99 : 101;
        }
    }
}
