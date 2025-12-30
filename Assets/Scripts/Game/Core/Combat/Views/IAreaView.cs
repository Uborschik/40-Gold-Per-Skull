using Game.Core.Combat.Grid;

namespace Game.Core.Combat.Views
{
    public interface IAreaView
    {
        void PaintArea(IArea area);
        void ClearArea();
    }
}
