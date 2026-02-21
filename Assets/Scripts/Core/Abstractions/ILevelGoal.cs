using TMPro;

namespace APGame.Abstractions
{
    public interface ILevelGoal
    {
        public void Initialize();
        public void Update();
        string GetLevelText();
    }
}
