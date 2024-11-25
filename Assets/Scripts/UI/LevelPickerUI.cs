using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonSelectorUI), typeof(Inventory))]
public class LevelPickerUI : Singleton<LevelPickerUI>
{
    [SerializeField] private GameObject _offenseLevel;
    [SerializeField] private TemporalLevel _offenseLevelInstance;
    [SerializeField] private GameObject _defenseLevel;
    [SerializeField] private TemporalLevel _defenseLevelInstance;
    [SerializeField] private CarbonSlot _carbonSlot;
    [SerializeField] private Button _travelButton;
    private bool _defenseUnlocked;
    private ButtonSelectorUI _buttonSelector;
    private Inventory _inventory;
    private CinemachineConfiner2D _cinemachineConfiner;

    public bool DefenseUnlocked => _defenseUnlocked;

    void Start()
    {
        CloseLevelPicker();

        _buttonSelector = GetComponent<ButtonSelectorUI>();
        _inventory = GetComponent<Inventory>();

        _travelButton.onClick.AddListener(() => TravelPlayer());

        _cinemachineConfiner = GameObject.Find("VirtualCamera").GetComponent<CinemachineConfiner2D>();
    }

    void Update()
    {
        
    }

    public void UnlockDefenseLevel()
    {
        if (!_defenseUnlocked)
        {
            _defenseUnlocked = true;
            _defenseLevel.transform.GetChild(0).GetComponent<Button>().interactable = true;
            if (ColorUtility.TryParseHtmlString("#C8854C", out var color))
            {
                var colors = _defenseLevel.transform.GetChild(0).GetComponent<Button>().colors;
                colors.normalColor = color;
            }
            _defenseLevel.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void CloseLevelPicker()
    {
        gameObject.SetActive(false);
    }

    public void OpenLevelPicker()
    {
        gameObject.SetActive(true);
        UpdateItemsInLevelPickerUI();
    }

    public void TravelPlayer()
    {
        if (_buttonSelector.SelectedButton == _offenseLevel.GetComponentInChildren<Button>())
        {
            InstantiateLevel(_offenseLevelInstance);
            CloseLevelPicker();
        } else if (_buttonSelector.SelectedButton == _defenseLevel.GetComponentInChildren<Button>())
        {
            InstantiateLevel(_defenseLevelInstance);
            CloseLevelPicker();
        } else
        {
            // No button selected
            Debug.Log("No level selected");
        }
    }

    private void InstantiateLevel(TemporalLevel level)
    {
        Instantiate(level, GameObject.Find("LevelZone").transform);

        Player.Instance.SetCurrentZone(TemporalLevel.Instance.gameObject);
        TemporalLevel.Instance.UpdateTileManager();
        if (_carbonSlot.Stack != null) TemporalLevel.Instance.SetDifficulty(_carbonSlot.Stack.Amount);
        RemoveItemStack(_carbonSlot.Stack);
        Player.Instance.transform.SetPositionAndRotation(TemporalLevel.Instance.Spawner.position, Player.Instance.transform.rotation);
        if (_cinemachineConfiner != null && TemporalLevel.Instance.CameraConfiner != null)
        {
            _cinemachineConfiner.m_BoundingShape2D = TemporalLevel.Instance.CameraConfiner;
            CinemachineVirtualCamera vcam = _cinemachineConfiner.GetComponent<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.OnTargetObjectWarped(Player.Instance.transform, TemporalLevel.Instance.Spawner.transform.position - Player.Instance.transform.position);

                vcam.PreviousStateIsValid = false;
            }
        }
    }

    public ItemStack AddItemStack(ItemStack newStack)
    {
        ItemStack addedItemStack = _inventory.AddItemStack(newStack);
        UpdateItemsInLevelPickerUI();
        return addedItemStack;
    }
    
    public void RemoveItemStack(ItemStack newStack)
    {
        _inventory.RemoveItemStack(newStack);
        UpdateItemsInLevelPickerUI();
    }

    public void UpdateItemsInLevelPickerUI()
    {
        if (!gameObject.activeSelf) return;
        if (_inventory.Stacks.Count == 0)
        {
            Image itemImage = _carbonSlot.transform.GetChild(0).GetComponent<Image>();
            Color color = itemImage.color;
            color.a = 0f;
            itemImage.color = color;
            TextMeshProUGUI itemStack = _carbonSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            itemStack.text = "";
            _carbonSlot.Stack = null;
            return;
        }
        
        var stack = _inventory.Stacks[0];

        if (stack.Amount == 0)
        {
            Image itemImage = _carbonSlot.transform.GetChild(0).GetComponent<Image>();
            Color color = itemImage.color;
            color.a = 0f;
            itemImage.color = color;
            TextMeshProUGUI itemStack = _carbonSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            itemStack.text = "";
            _carbonSlot.Stack = null;
        } else
        {
            var image = _carbonSlot.transform.GetChild(0).GetComponent<Image>();
            image.sprite = stack.Item.Sprite;
            var color = image.color;
            color.a = 1f;
            image.color = color;

            var amountText = _carbonSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            amountText.text = stack.Amount.ToString();
            _carbonSlot.Stack = stack;
        }
    }
}
