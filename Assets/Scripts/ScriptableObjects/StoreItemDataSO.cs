using UnityEngine;

[CreateAssetMenu(fileName = "StoreItemData", menuName = "Store/StoreItemData")]
public class StoreItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int amount; 
    public int duration;
    public int price;
    public string description;
}

public enum ItemType
{
    Health,        
    Mana,          
    AttackBoost,   
    DefenseBoost   
}
