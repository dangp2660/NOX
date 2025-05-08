using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public float currentHP;
    public float currentDarkEnergy;
    public float maxHP;
    public float maxDarkEnergy;
    public string currentMap;
    public SaveData() { }
    public SaveData(Vector3 playerPos, float currentHP, float currentDarkEnergy, float maxHP, float maxDarkEnergy, string currentMap)
    {
        this.playerPos = playerPos;
        this.currentHP = currentHP;
        this.currentDarkEnergy = currentDarkEnergy;
        this.maxHP = maxHP;
        this.maxDarkEnergy = maxDarkEnergy;
        this.currentMap = currentMap;
    }
}
