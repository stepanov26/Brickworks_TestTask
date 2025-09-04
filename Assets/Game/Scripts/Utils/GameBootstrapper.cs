using Brickworks.Model.Skills;
using Brickworks.UI.Model.Skills;
using Brickworks.UI.View.Skills;
using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField]
    private SkillsScreenView _skillsScreenViewPrefab;
    
    private SkillsModel _skillsModel;
    private ResourcesModel _resourcesModel;

    [Inject]
    private void Construct(SkillsModel skillsModel, ResourcesModel resourcesModel)
    {
        _skillsModel = skillsModel;
        _resourcesModel = resourcesModel;
    }
    
    private void Start()
    {
        var canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        var screen = Instantiate(_skillsScreenViewPrefab, canvas);
        screen.Init(new SkillsScreenModel(_skillsModel, _resourcesModel));
    }
}
