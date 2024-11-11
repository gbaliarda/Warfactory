using UnityEngine;

namespace Entities
{
    public class WeaponEnemy : Enemy
    {
        [SerializeField] protected MonoBehaviour _weapon;
        public IWeapon Weapon => _weapon as IWeapon;

        [SerializeField] private bool _facingRight = true;

        protected override void Attack()
        {
            var player = Player.Instance;
            if (player.IsDead) return;
            FaceTarget(player.transform);
            
            if (_weapon == null || _weapon is not IWeapon)
            {
                Debug.LogError("Weapon missing", this);
                return;
            }
            
            var dir = (player.transform.position - transform.position).normalized;
            Weapon.Attack(dir);
        }

        protected override void Chase()
        {
            if (Player.Instance.IsDead) return;
            base.Chase();
            
            var player = Player.Instance;
            FaceTarget(player.transform);
        }
        
        protected void FaceTarget(Transform target)
        {
            if (target.position.x > transform.position.x && !_facingRight)
            {
                Flip();
            }
            else if (target.position.x < transform.position.x && _facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            _facingRight = !_facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void OnValidate()
        {
            if(_weapon == null || _weapon is not IWeapon)
                Debug.LogError("Weapon missing", this);
        }
    }
}