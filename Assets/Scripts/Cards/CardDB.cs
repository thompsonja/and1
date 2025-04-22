using UnityEngine;
[System.Serializable]

public class CardDB : ScriptableObject
{
    public int id;
    public new string name;
    public int cost;
    public string description;

    public string art;

    // public Card()
    // {
    //     return 
    // }

    // public Card(int id, string name, int cost, string description, string art)
    // {
    //     this.id = id;
    //     this.name = name;
    //     this.cost = cost;
    //     this.description = description;
    //     this.art = art;
    // }
}
