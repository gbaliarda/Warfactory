using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TemporalLevel : Singleton<TemporalLevel>, IZone
{
    [SerializeField] private Transform _spawner;
    [SerializeField] private Collider2D _cameraConfiner;
    [SerializeField] private Tilemap _interactableMap;
    [SerializeField] private Tilemap _uiHoverMap;
    [SerializeField] private Tilemap _resourceMap;
    [SerializeField] private Tilemap _grassMap;
    [SerializeField] private Tilemap _cosmeticMap;
    [SerializeField] private LevelType _levelType;

    public Transform Spawner => _spawner;
    public Collider2D CameraConfiner => _cameraConfiner;
    [SerializeField] private Transform _objectsContainer;
    public Transform ObjectsContainer => _objectsContainer;

    private int _difficulty = 0;

    public int Difficulty => _difficulty;

    void Start()
    {
        UpdateTileManager();
    }

    void Update()
    {
        
    }

    public void UpdateTileManager()
    {
        TileManager.Instance.SetInteractableMap(_interactableMap);
        TileManager.Instance.SetUIHoverMap(_uiHoverMap);
        TileManager.Instance.SetResourceMap(_resourceMap);        
        TileManager.Instance.SetGrassMap(_grassMap);
        TileManager.Instance.SetCosmeticMap(_cosmeticMap);
        TileManager.Instance.InstantiateInteractableMap();
    }

    private void OnDestroy()
    {
        TileManager.Instance.RestoreBaseTilemaps();
    }

    public void SetDifficulty(int difficulty)
    {
        _difficulty = difficulty;
    }

    public void CompleteLevel()
    {
        ProgressManager.Instance.LevelCompleted(_levelType, _difficulty);
        switch (_levelType)
        {
            case LevelType.Intro:
                GameManager.Instance.CompleteTutorial();
                break;
            case LevelType.Offense:
                LevelPickerUI.Instance.UnlockDefenseLevel();
                break;
        }
    }
}
