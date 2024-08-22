using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Player : Actor
{
    private static Player instance;
    public static Player Instance { get { return instance; } }

    protected MoveController moveController;

    [SerializeField] private MonoBehaviour _weapon;

    #region KEYBINDINGS
    [SerializeField] private KeyCode _shoot = KeyCode.Mouse0;
    [SerializeField] private KeyCode _interact = KeyCode.Mouse1;
    [SerializeField] private KeyCode _moveForward = KeyCode.W;
    [SerializeField] private KeyCode _moveBack = KeyCode.S;
    [SerializeField] private KeyCode _moveLeft = KeyCode.A;
    [SerializeField] private KeyCode _moveRight = KeyCode.D;
    [SerializeField] private KeyCode _hotbarSlot1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode _hotbarSlot2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode _hotbarSlot3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode _hotbarSlot4 = KeyCode.Alpha4;
    [SerializeField] private KeyCode _hotbarSlot5 = KeyCode.Alpha5;
    [SerializeField] private KeyCode _hotbarSlot6 = KeyCode.Alpha6;
    #endregion

    #region COMMANDS
    private MoveCommand _cmdMovementForward;
    private MoveCommand _cmdMovementBack;
    private MoveCommand _cmdMovementLeft;
    private MoveCommand _cmdMovementRight;
    #endregion

    protected void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        moveController = GetComponent<MoveController>();

        /*EventManager.Instance.OnHotbarItemSelect += OnHotbarItemSelect;*/
    }

    new void Start()
    {
        _cmdMovementForward = new MoveCommand(moveController, transform.up);
        _cmdMovementBack = new MoveCommand(moveController, -transform.up);
        _cmdMovementLeft = new MoveCommand(moveController, -transform.right);
        _cmdMovementRight = new MoveCommand(moveController, transform.right);

    }

    new void Update()
    {
        if (Input.GetKey(_moveForward)) EventQueueManager.Instance.AddEvent(_cmdMovementForward);
        if (Input.GetKey(_moveBack)) EventQueueManager.Instance.AddEvent(_cmdMovementBack);
        if (Input.GetKey(_moveRight)) EventQueueManager.Instance.AddEvent(_cmdMovementRight);
        if (Input.GetKey(_moveLeft)) EventQueueManager.Instance.AddEvent(_cmdMovementLeft);
        if (Input.GetKey(_hotbarSlot1)) EventManager.Instance.EventHotbarSlotChange(0);
        if (Input.GetKey(_hotbarSlot2)) EventManager.Instance.EventHotbarSlotChange(1);
        if (Input.GetKey(_hotbarSlot3)) EventManager.Instance.EventHotbarSlotChange(2);
        if (Input.GetKey(_hotbarSlot4)) EventManager.Instance.EventHotbarSlotChange(3);
        if (Input.GetKey(_hotbarSlot5)) EventManager.Instance.EventHotbarSlotChange(4);
        if (Input.GetKey(_hotbarSlot6)) EventManager.Instance.EventHotbarSlotChange(5);

        if (Input.GetKeyDown(_interact))
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                Vector3 clickPosition = hit.point;
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.TryGetComponent<Tilemap>(out var tilemap))
                {
                    Vector3Int cellPosition = tilemap.WorldToCell(clickPosition);

                    if (TileManager.Instance.IsInteractable(cellPosition))
                    {
                        TileManager.Instance.SetInteractable(cellPosition);
                    }
                }

                //EventQueueManager.Instance.AddEvent(_cmdLeftClick);
            }
        }


        if (Input.GetKeyDown(_shoot))
        {
            if (!EventSystem.current.IsPointerOverGameObject() && _weapon.gameObject.activeSelf && _weapon.GetComponent<IWeapon>() != null)
            {
                ShootWeapon();
            }
        }
    }

    private void OnHotbarItemSelect(GameObject gameObject)
    {

    }

    private void ShootWeapon()
    {

        (_weapon as IWeapon).Attack();
    }
}
