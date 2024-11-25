using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Loot Table", fileName = "New Loot Table")]
public class LootTable : ScriptableObject
{
    [Header("Guaranteed")]
    [SerializeField] private ItemStack[] _guaranteedItems;

    [Header("Random")]
    [SerializeField] private RandomEntry[] _randomEntries;
    [SerializeField] private int _randomRolls = 1;

    [Header("Auto-Generated")]
    [SerializeField] private int _totalWeight;


    public ItemStack[] GetLoot()
    {
        var rewards = new List<ItemStack>(_guaranteedItems.Select(i => {
            var newItem = i.Clone();
            newItem.IncreaseAmount(TemporalLevel.Instance.Difficulty);
            return newItem;
        }));

        for (var i = 0; i < _randomRolls + TemporalLevel.Instance.Difficulty; i++)
        {
            var r = UnityEngine.Random.Range(0, _totalWeight);

            foreach (var entry in _randomEntries)
            {
                r -= entry.Weight;
                if (r > 0) continue;

                rewards.Add(entry.Stack.Clone());
                break;
            }
        }

        return rewards.ToArray();
    }

    private void OnValidate()
    {
        _totalWeight = _randomEntries.Sum(e => e.Weight);
    }


    [Serializable]
    private class RandomEntry
    {
        [SerializeField] private ItemStack _stack;
        [SerializeField] private int _weight = 100;

        public ItemStack Stack => _stack;
        public int Weight => _weight;
    }
}
