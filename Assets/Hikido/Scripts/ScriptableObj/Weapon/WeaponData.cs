using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType 
{
    Sword,
    Staff,
    Bow,
}

[CreateAssetMenu(fileName ="WeaponData" , menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("アイテムキー")]
    public string itemKeyName;

    [Header("武器情報")]
    public GameObject weaponPrefab;

    [Header("武器情報")]
    public WeaponType type;

    [Header("追加性能")]
    //武器の追加性能 -> こちらでは使わないかも
    public float plusAttack = 1.0f;
    public float attackRange = 1.0f;

    [Header("アタッチ位置")]
    public Vector3 attatchLocalPos = Vector3.zero;
    public Vector3 attachLocalRotationEules = Vector3.zero;

    [Header("左手（弓装備専用)")]
    public Vector3 leftAttachLocalPos;
    public Vector3 leftAttachLocalRotationEules;

}
