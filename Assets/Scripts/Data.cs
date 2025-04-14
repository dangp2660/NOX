using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewCharacters", menuName ="Character Stats")]
public class Data : ScriptableObject
{
    [SerializeField] private float hp;
    [SerializeField] private float dame;
    [SerializeField] private float defend;
}
