using Brickworks.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SkillItemData), menuName = "ScriptableObjects/SkillItemData")]
public class SkillItemData : ScriptableObject
{
    [SerializeField]
    private string _id;

    [SerializeField] 
    private string _title;

    [SerializeField]
    private ResourceData _price;

    [SerializeField] 
    private SkillItemData[] _previousSkills;
    
    [SerializeField]
    private SkillItemData[] _nextSkills;

    public string Id => _id;
    public string Title => _title;
    public ResourceData Price => _price;
    public SkillItemData[] PreviousSkills => _previousSkills;
    public SkillItemData[] NextSkills => _nextSkills;
}
