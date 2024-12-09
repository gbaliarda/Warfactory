    
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ProgressUI : MonoBehaviour
{
    [FormerlySerializedAs("_level1Stars")] [SerializeField] private Transform _offenseStars;
    [FormerlySerializedAs("_level2Stars")] [SerializeField] private Transform _defenseStars;

    private void OnEnable()
    {
        ProgressManager.Instance.OnProgressChange += UpdateProgress;
        UpdateProgress();
    }

    private void OnDisable()
    {
        ProgressManager.Instance.OnProgressChange -= UpdateProgress;
    }

    private void UpdateProgress()
    {
        UpdateStars(_offenseStars, ProgressManager.Instance.OffenseProgress);
        UpdateStars(_defenseStars, ProgressManager.Instance.DefenseProgress);
    }

    private void UpdateStars(Transform stars, int progress)
    {
        // Enable stars based on progress
        for (int i = 0; i < stars.childCount; i++)
        {
            stars.GetChild(i).gameObject.SetActive(i < progress);
        }
    }

    private void OnValidate()
    {
        UpdateProgress();
    }
}
