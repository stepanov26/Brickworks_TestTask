using Brickworks.Utils;

namespace Brickworks.UI.View.Skills
{
    public interface ISkillViewModel
    {
        string Id { get; }
        string Title { get; }
        IReactiveProperty<bool> IsPurchased { get; }
        
        void Purchase();
        void Forget();
    }
}