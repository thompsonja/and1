using System.Collections.Generic;
using UnityEngine;
using SerializeReferenceEditor;


[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Energy { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string Title { get; private set; }

    // Unity cannot serialize abstract classes, use SerializeReference instead
    [field: SerializeReference, SR] public List<Effect> Effects { get; set; } = new List<Effect>();
}
