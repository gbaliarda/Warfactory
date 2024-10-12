using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Player : Actor, IBuffable
{
    public static Player Instance { get; private set; }

    private Vector2 moveDirection;

    [SerializeField] private MonoBehaviour _weapon;
    [SerializeField] private MonoBehaviour _potion;
    [SerializeField] private MonoBehaviour _potionBuilding;
    [SerializeField] private MonoBehaviour _chestBuilding;
    [SerializeField] private MonoBehaviour _slider;
    [SerializeField] private MonoBehaviour _extractor;
    [SerializeField] private ActorStats _baseStats;
    [SerializeField] private Transform _hotbarItems;
    [SerializeField] private Transform _buildHotbarItems;
    
    // SFX
    [SerializeField] private string deadSound = "PlayerDead";

    public bool BuildingMode => _buildingMode;

    private bool _buildingMode = false;
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
    [SerializeField] private KeyCode _inventory = KeyCode.I;
    [SerializeField] private KeyCode _buildModeKey = KeyCode.Q;
    [SerializeField] private KeyCode _rotateBuilding = KeyCode.R;
    #endregion

    #region PARAMS
    [Header("Params")]
    [SerializeField] private float _shootOriginDistance = 5f;
    #endregion

    public GameObject ObjectInHand => _objectInHand;

    private GameObject _objectInHand;

    private int _buildingRotation = 1;

    [SerializeField] private GameObject grave;


    protected void Awake()
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

        buffs ??= new();

        EventManager.Instance.OnHotbarItemSelect += OnHotbarItemSelect;

        stats = Instantiate(_baseStats);
    }

    #region MOVEMENT_INPUT
    void InputMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    #endregion

    [SerializeField] private GameObject _levelPortal;
    [SerializeField] private GameObject _basePortal;

    new void Update()
    {
        InputMovement();

        if (Input.GetKey(_hotbarSlot1)) EventManager.Instance.EventHotbarSlotChange(0);
        if (Input.GetKey(_hotbarSlot2)) EventManager.Instance.EventHotbarSlotChange(1);
        if (Input.GetKey(_hotbarSlot3)) EventManager.Instance.EventHotbarSlotChange(2);
        if (Input.GetKey(_hotbarSlot4)) EventManager.Instance.EventHotbarSlotChange(3);
        if (Input.GetKey(_hotbarSlot5)) EventManager.Instance.EventHotbarSlotChange(4);
        if (Input.GetKey(_hotbarSlot6)) EventManager.Instance.EventHotbarSlotChange(5);
        if (Input.GetKeyDown(KeyCode.P)) Instantiate(_levelPortal, transform.position + transform.rotation * Vector3.up * 2, Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.O)) Instantiate(_basePortal, transform.position + transform.rotation * Vector3.up * 2, Quaternion.identity);
        if (Input.GetKeyDown(_buildModeKey))
        {
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

        if (Input.GetKeyDown(_interact) && _buildingMode)
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerToIgnore = LayerMask.GetMask("Camera");
            int layerMask = ~layerToIgnore;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                Vector3 clickPosition = hit.point;
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.TryGetComponent<Tilemap>(out var tilemap))
                {
                    Vector3Int cellPosition = tilemap.WorldToCell(clickPosition);

                    if (TileManager.Instance.IsInteractable(cellPosition))
                    {
                        if (!EventSystem.current.IsPointerOverGameObject() && _potionBuilding.gameObject.activeSelf && _potionBuilding.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_potionBuilding as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2, _buildingRotation);
                        }
                        if (_slider.gameObject.activeSelf && _slider.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_slider as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2, _buildingRotation);
                        }
                        if (_chestBuilding.gameObject.activeSelf && _chestBuilding.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_chestBuilding as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2, _buildingRotation);
                        }
                        if(_extractor.gameObject.activeSelf && _extractor.GetComponent<IBuilding>() != null)
                        {
                            TileManager.Instance.SetOccupied(cellPosition);
                            (_extractor as IBuilding).Build(tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2, _buildingRotation);
                        }
                    }
                    /*if (TileManager.Instance.IsInteractable(cellPosition))
                    {
                        TileManager.Instance.SetInteractable(cellPosition);
                    }*/
                }

                //EventQueueManager.Instance.AddEvent(_cmdLeftClick);
            }
        }


        if (Input.GetKeyDown(_shoot) && !_buildingMode)
        {
            if (!EventSystem.current.IsPointerOverGameObject() && _weapon.gameObject.activeSelf && _weapon.GetComponent<IWeapon>() != null)
            {
                ShootWeapon();
            }
            if (_potion.gameObject.activeSelf && _potion.GetComponent<IPotion>() != null)
            {
                UsePotion();
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

    void Move()
    {
        Rigidbody.velocity = moveDirection * stats.MovementSpeed;
    }

    private void OnHotbarItemSelect(GameObject gameObject)
    {

    }

    private void ShootWeapon()
    {
        var cam = Camera.main;
        if(!cam) return;

        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        var dir = (mousePosition - (Vector2)transform.position).normalized;
        var origin = (Vector2)transform.position + _shootOriginDistance * dir;
        ((IWeapon)_weapon).Attack(origin, dir);
    }

    private void UsePotion()
    {
        ((IPotion)_potion).Buff();
    }

    public void AddBuff(IPotion potion)
    {
        if (isDead) return;
        stats.AddStats(potion.PotionStats);
        buffs.Add(potion);
    }

    public void RemoveBuff(IPotion potion)
    {
        if (isDead) return;
        stats.RemoveStats(potion.PotionStats);
        buffs.Remove(potion);
    }
    
    // Override the Die method from Actor to display the death animation
    protected override IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        
        GameObject graveInstance = Instantiate(grave, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX(deadSound);
        
        Destroy(gameObject);
    }
}
