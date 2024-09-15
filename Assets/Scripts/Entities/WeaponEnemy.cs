using UnityEngine;

namespace Entities
{
    public class WeaponEnemy : Enemy
    {
        [SerializeField] private MonoBehaviour _weapon;
        public IWeapon Weapon => _weapon as IWeapon;

        protected override void Attack()
        {
            var player = Player.Instance;
            LookAt(player.transform);
            
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
            base.Chase();
            
            var player = Player.Instance;
            LookAt(player.transform);
        }
        
        private void LookAt(Transform target)
        {
            transform.right = (target.position - transform.position).normalized;
        }

        private void OnValidate()
        {
            if(_weapon == null || _weapon is not IWeapon)
                Debug.LogError("Weapon missing", this);
        }
    }
}