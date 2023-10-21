using AleVerDes.LeoEcsLiteZoo;

namespace CoreLogic.Common
{
    public interface IAbility : IConvertToEntity
    {
        void Execute(Actor other = null)
        {
            
        }
    }
}