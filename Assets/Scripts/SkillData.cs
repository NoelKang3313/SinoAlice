using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Skill Data")]
public class SkillData : ScriptableObject
{
    public Sprite SkillIcon;
    public string SkillName;
    public string SkillDescription;
    public int CostMP;
    public string SkillUserName;    
}
