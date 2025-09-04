using Brickworks.Model.Skills;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SkillsModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<ResourcesModel>().AsSingle();
    }
}