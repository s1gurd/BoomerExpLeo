using AleVerDes.LeoEcsLiteZoo;

namespace CoreLogic.Common
{
    public interface IAbility : IConvertToEntity
    {
        bool Execute(Actor other = null)
        {
            return true;
        }
    }
}