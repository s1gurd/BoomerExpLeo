using AleVerDes.LeoEcsLiteZoo;

namespace CoreLogic
{
    public class MainEcsModuleInstaller : IEcsModuleInstaller
    {
        public IEcsModule Install()
        {
            var module = new EcsModule();
        
            module
                .AddFeature(new DebugFeature())
                ;

            return module;
        }
    }
}