using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacters", menuName = "Character Stats")]
public class Data : ScriptableObject
{
    [SerializeField] private float hp;
    [SerializeField] private float dame;
    [SerializeField] private float defend;

    public float Hp
    {
        get => hp;
        set => hp = Mathf.Max(0, value); // Ngăn máu xuống âm
    }

    public float Dame => dame;
    public float Defend => defend;
}
