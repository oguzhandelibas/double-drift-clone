using DoubleDrift.UIModule;
using Zenject;

namespace DoubleDrift
{
    public class ZenjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InfinityPathManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
