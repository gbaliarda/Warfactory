using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Player : Actor, IBuffable
{
    public static Player Instance { get; private set; }

    private Vector2 _moveDirection;
    [SerializeField] public GameObject interactableIcon;
    
    private Vector2 boxSize = new Vector2(0.1f, 1f);
    
    [SerializeField] private MonoBehaviour _pistol;
    [SerializeField] private MonoBehaviour _shotgun;
    [SerializeField] private MonoBehaviour _assaultRifle;
    [SerializeField] private MonoBehaviour _ragePotion;
    [SerializeField] private MonoBehaviour _healthPotion;
    [FormerlySerializedAs("_potionBuilding")] [SerializeField] private MonoBehaviour _shotgunBulletBuilding;
    [SerializeField] private MonoBehaviour _assaultRifleBulletBuilding;
    [FormerlySerializedAs("_potionFactory")] [SerializeField] private MonoBehaviour _ragePotionFactory;
    [SerializeField] private MonoBehaviour _healthPotionFactory;
    [SerializeField] private MonoBehaviour _chestBuilding;
    [SerializeField] private MonoBehaviour _slider;
    [SerializeField] private MonoBehaviour _extractor;
    [SerializeField] private MonoBehaviour _turret;
    [SerializeField] private ActorStats _baseStats;
    [SerializeField] private Transform _hotbarItems;
    [SerializeField] private Transform _buildHotbarItems;
    private int _currentHotbarItemIndex = 0;

    // SFX
    [SerializeField] private string deadSound = "PlayerDead";

    public bool BuildingMode => _buildingMode;
    public bool DeleteBuildingMode => _deleteBuildingMode;

    private bool _buildingMode = false;
    private bool _deleteBuildingMode = false;
    public Transform HotbarItems => _hotbarItems;
    public Transform BuildHotbarItems => _buildHotbarItems;
    public Rigidbody2D Rigidbody { get; private set; }

    protected List<IPotion> buffs;
    public List<IPotion> Buffs => buffs;

    #region KEYBINDINGS
    [Header("Keybindings")]
    [SerializeField] private KeyCode _shoot = KeyCode.Mouse0;
    [SerializeField] private KeyCode _interact = KeyCode.Mouse1;

    [SerializeField] private KeyCode _hotbarSlot1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode _hotbarSlot2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode _hotbarSlot3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode _hotbarSlot4 = KeyCode.Alpha4;
    [SerializeField] private KeyCode _hotbarSlot5 = KeyCode.Alpha5;
    [SerializeField] private KeyCode _hotbarSlot6 = KeyCode.Alpha6;
    [SerializeField] private KeyCode _hotbarSlot7 = KeyCode.Alpha7;
    [SerializeField] private KeyCode _hotbarSlot8 = KeyCode.Alpha8;
    [SerializeField] private KeyCode _inventory = KeyCode.I;
    [SerializeField] private KeyCode _buildModeKey = KeyCode.Q;
    [SerializeField] private KeyCode _rotateBuilding = KeyCode.R;
    [SerializeField] private KeyCode _deleteBuilding = KeyCode.Z;
    [SerializeField] private KeyCode _read = KeyCode.E;
    
    #endregion

    #region PARAMS
    [Header("Params")]
    [SerializeField] private float _shootOriginDistance = 5f;
    #endregion

    public GameObject ObjectInHand => _objectInHand;

    private GameObject _objectInHand;

    private int _buildingRotation = 1;

    private GameObject _currentZone;

    public GameObject CurrentZone => _currentZone;
    [SerializeField] private GameObject grave;


    protected override void Awake()
    {
        if (Instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        buffs ??= new List<IPotion>();

        EventManager.Instance.OnHotbarItemSelect += OnHotbarItemSelect;

        stats = Instantiate(_baseStats);
        _currentZone = GameObject.Find("Base");
    }

    #region MOVEMENT_INPUT
    void InputMovement()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(moveX, moveY).normalized;
    }

    public void SetCurrentZone(GameObject zone)
    {
        _currentZone = zone;
    }
    #endregion

    [SerializeField] private GameObject _levelPortal;
    [SerializeField] private GameObject _basePortal;
    [SerializeField] private GameObject _enemy; // TODO: Delete, just for testing
    protected override void Update()
    {
        if (isDead) return;
        InputMovement();

        if (Input.GetKeyDown(_read))
            checkInteractable();
        
        if (Input.GetKeyDown(_hotbarSlot1)) hotbarSlotChange(0);
        if (Input.GetKeyDown(_hotbarSlot2)) hotbarSlotChange(1);
        if (Input.GetKeyDown(_hotbarSlot3)) hotbarSlotChange(2);
        if (Input.GetKeyDown(_hotbarSlot4)) hotbarSlotChange(3);
        if (Input.GetKeyDown(_hotbarSlot5)) hotbarSlotChange(4);
        if (Input.GetKeyDown(_hotbarSlot6)) hotbarSlotChange(5);
        if (Input.GetKeyDown(_hotbarSlot7)) hotbarSlotChange(6);
        if (Input.GetKeyDown(_hotbarSlot8)) hotbarSlotChange(7);
        if (Input.GetKeyDown(KeyCode.P)) Instantiate(_levelPortal, transform.position + transform.rotation * Vector3.up * 2, Quaternion.identity, CurrentZone.transform);
        if (Input.GetKeyDown(KeyCode.O)) Instantiate(_basePortal, transform.position + transform.rotation * Vector3.up * 2, Quaternion.identity, CurrentZone.transform);
        if (Input.GetKeyDown(KeyCode.L)) Instantiate(_enemy, transform.position + transform.rotation * Vector3.up * 2, Quaternion.identity, CurrentZone.transform);
        if (Input.GetKeyDown(_deleteBuilding))
        {
            if (_buildingMode)
            {
                _deleteBuildingMode = !_deleteBuildingMode;
                EventManager.Instance.EventDeleteBuildModeActive(_deleteBuildingMode);
            }
        }
        if (Input.GetKeyDown(_buildModeKey))
        {
            hotbarSlotChange(0);
            _buildingMode = !_buildingMode;
            _hotbarItems.gameObject.SetActive(!_buildingMode);
            _buildHotbarItems.gameObject.SetActive(_buildingMode);
            EventManager.Instance.EventBuildModeActive(_buildingMode);
        }
        if(Input.GetKeyDown(_rotateBuilding))
        {
            _buildingRotation += 1;
            if (_buildingRotation == 5)
                _buildingRotation = 1;
        }

        if (Input.GetKeyDown(_inventory))
        {
            if (InventoryManager.Instance.IsOpen)
                EventManager.Instance.EventCloseInventoryUI();
            else
                EventManager.Instance.EventOpenInventoryUI();
        }

        if (Input.GetKeyDown(_interact) && _buildingMode && _deleteBuildingMode)
        {
            var cam = Camera.main;
            if (!cam) return;

            var mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            int layerToIgnore = LayerMask.GetMask("Camera");
            int layerMask = ~layerToIgnore;
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                var clickedObject = hit.collider.gameObject;

                if (clickedObject.TryGetComponent<IDestroyable>(out var destroyable))
                {
                    destroyable.Destroy();
                    TileManager.Instance.SetUnoccupied(hit.point);
                }
            }
        }

        if (Input.GetKeyDown(_interact) && _buildingMode && !_deleteBuildingMode)
        {
            var cam = Camera.main;
            if(!cam) return;

            var mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            int layerToIgnore = LayerMask.GetMask("Camera");
            int layerMask = ~layerToIgnore;
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                Vector3 clickPosition = hit.point;
                var clickedObject = hit.collider.gameObject;

                if (clickedObject.TryGetComponent<Tilemap>(out var tilemap))
                {
                    var cellPosition = tilemap.WorldToCell(clickPosition);

                    if (TileManager.Instance.IsInteractable(cellPosition))
                    {
                        if (!EventSystem.current.IsPointerOverGameObject() && _shotgunBulletBuilding.gameObject.activeSelf && _shotgunBulletBuilding.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_shotgunBulletBuilding as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -1), _buildingRotation);
                        }
                        if (!EventSystem.current.IsPointerOverGameObject() && _assaultRifleBulletBuilding.gameObject.activeSelf && _assaultRifleBulletBuilding.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_assaultRifleBulletBuilding as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -1), _buildingRotation);
                        }
                        if (!EventSystem.current.IsPointerOverGameObject() && _turret.gameObject.activeSelf && _turret.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_turret as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -1), 1);
                        }
                        if (!EventSystem.current.IsPointerOverGameObject() && _ragePotionFactory.gameObject.activeSelf && _ragePotionFactory.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_ragePotionFactory as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -1), _buildingRotation);
                        }
                        if (!EventSystem.current.IsPointerOverGameObject() && _healthPotionFactory.gameObject.activeSelf && _healthPotionFactory.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_healthPotionFactory as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -1), _buildingRotation);
                        }
                        if (_slider.gameObject.activeSelf && _slider.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_slider as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -0.25f), _buildingRotation);
                        }
                        if (_chestBuilding.gameObject.activeSelf && _chestBuilding.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_chestBuilding as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -1), 1); // Can not rotate chest
                        }
                        if(_extractor.gameObject.activeSelf && _extractor.GetComponent<IBuilding>() != null)
                        {
                            if (TileManager.Instance.IsResource(cellPosition))
                            {
                                var tile = TileManager.Instance.GetResourceTile(cellPosition);

                                TileManager.Instance.SetOccupied(cellPosition);
                                var go = (_extractor as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2 + new Vector3(0, 0, -1), _buildingRotation);
                                if (!go) return;
                                
                                var extractor = go.GetComponent<ExtractorBuilding>();
                                extractor.Tile = tile;
                            }
                        }
                    }
                }

            }
        }

        if (Input.GetKeyDown(_interact) && !_deleteBuildingMode)
        {
            var cam = Camera.main;
            if (!cam) return;

            var mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            int layerToIgnore = LayerMask.GetMask("Camera");
            int layerMask = ~layerToIgnore;
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                var clickedObject = hit.collider.gameObject;
                if (clickedObject.TryGetComponent<ChestBuilding>(out var chestBuildingClicked))
                {
                    Debug.Log(chestBuildingClicked);
                    chestBuildingClicked.OpenChest();
                }
            }
        }


        if (Input.GetKey(_shoot) && !_buildingMode)
        {
            if (!EventSystem.current.IsPointerOverGameObject() && _shotgun.gameObject.activeSelf && _shotgun.GetComponent<IWeapon>() != null)
            {
                ShootWeapon(_shotgun.GetComponent<IWeapon>());
            }
            if (!EventSystem.current.IsPointerOverGameObject() && _pistol.gameObject.activeSelf && _pistol.GetComponent<IWeapon>() != null)
            {
                ShootWeapon(_pistol.GetComponent<IWeapon>());
            }
            if (!EventSystem.current.IsPointerOverGameObject() && _assaultRifle.gameObject.activeSelf && _assaultRifle.GetComponent<IWeapon>() != null)
            {
                ShootWeapon(_assaultRifle.GetComponent<IWeapon>());
            }
            if (_ragePotion.gameObject.activeSelf && _ragePotion.GetComponent<IPotion>() != null)
            {
                UsePotion(_ragePotion.GetComponent<IPotion>());
            }            
            if (_healthPotion.gameObject.activeSelf && _healthPotion.GetComponent<IPotion>() != null)
            {
                UsePotion(_healthPotion.GetComponent<IPotion>());
            }
        }

        if (_buildingMode)
        {
            _objectInHand = GetFirstActiveChild(_buildHotbarItems);
        } else
        {
            _objectInHand = GetFirstActiveChild(_hotbarItems);
        }
    }

    private void hotbarSlotChange(int hotbarSlot)
    {
        int _oldCurrentSlot = _currentHotbarItemIndex;
        if (_oldCurrentSlot == hotbarSlot) return;
        if(_buildingMode && hotbarSlot < _buildHotbarItems.childCount)
        {
            _buildHotbarItems.GetChild(hotbarSlot).gameObject.SetActive(true);
            if (_oldCurrentSlot < _buildHotbarItems.childCount)
                _buildHotbarItems.GetChild(_oldCurrentSlot).gameObject.SetActive(false);
            _currentHotbarItemIndex = hotbarSlot;
            EventManager.Instance.EventHotbarSlotChange(hotbarSlot);
        } else if(!_buildingMode && hotbarSlot < _hotbarItems.childCount)
        {
            _hotbarItems.GetChild(hotbarSlot).gameObject.SetActive(true);
            if (_oldCurrentSlot < _hotbarItems.childCount)
                _hotbarItems.GetChild(_oldCurrentSlot).gameObject.SetActive(false);
            _currentHotbarItemIndex = hotbarSlot;
            EventManager.Instance.EventHotbarSlotChange(hotbarSlot);
        }
    }

    private GameObject GetFirstActiveChild(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Rigidbody.velocity = _moveDirection * stats.MovementSpeed;
    }

    private void OnHotbarItemSelect(GameObject go)
    {

    }

    private void ShootWeapon(IWeapon weapon)
    {
        var cam = Camera.main;
        if(!cam) return;

        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        var dir = (mousePosition - (Vector2)transform.position).normalized;
        var origin = (Vector2)transform.position + _shootOriginDistance * dir;
        weapon.Attack(origin, dir);
    }

    private void UsePotion(IPotion potion)
    {
        potion.Buff();
    }

    public void AddBuff(IPotion potion)
    {
        if (isDead) return;
        stats.AddStats(potion.PotionStats);
        if (life + potion.PotionStats.HealDamage > MaxLife)
        {
            Debug.Log("Healing damage");
            life = MaxLife;
        } else
        {
            Debug.Log("Healing damage");
            life += potion.PotionStats.HealDamage;
        }

        buffs.Add(potion);
    }

    public void RemoveBuff(IPotion potion)
    {
        if (isDead) return;
        stats.RemoveStats(potion.PotionStats);
        buffs.Remove(potion);
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;
        _moveDirection = new Vector2(0, 0).normalized;
        if (animator != null) animator.SetBool("Dead", true);

        // if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        DeathScreenUI.Instance.Popup();
    }

    public void Respawn()
    {
        buffs ??= new();

        stats = _baseStats;
        _currentZone = GameObject.Find("Base");
        isDead = false;
        life = MaxLife;
        _moveDirection = new Vector2(0, 0);

        transform.SetPositionAndRotation(Base.Instance.Spawner.position, transform.rotation);
        CinemachineConfiner2D _cinemachineConfiner = GameObject.Find("VirtualCamera").GetComponent<CinemachineConfiner2D>();
        if (_cinemachineConfiner != null && Base.Instance.CameraConfiner != null)
        {
            _cinemachineConfiner.m_BoundingShape2D = Base.Instance.CameraConfiner;
            CinemachineVirtualCamera vcam = _cinemachineConfiner.GetComponent<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.OnTargetObjectWarped(transform, Base.Instance.Spawner.position - transform.position);

                vcam.PreviousStateIsValid = false;
            }
        }

        if (TemporalLevel.Instance != null) Destroy(TemporalLevel.Instance.gameObject);
        if (LevelPortal.Instance != null) Destroy(LevelPortal.Instance.gameObject);
        if (BasePortal.Instance != null) Destroy(BasePortal.Instance.gameObject);
    }
    
    // Override the Die method from Actor to display the death animation
    protected override IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(deathAnimationDuration);

        GameObject graveInstance = Instantiate(grave, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX(deadSound);

        Destroy(gameObject);
    }
    
    #region READ

    public void OpenInteractableIcon()
    {
        if(interactableIcon != null)
            interactableIcon.SetActive(true);
    }
    
    public void CloseInteractableIcon()
    {
        if(interactableIcon != null)
            interactableIcon.SetActive(false);
    }

    private void checkInteractable()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
    }
    
    #endregion
    
}
