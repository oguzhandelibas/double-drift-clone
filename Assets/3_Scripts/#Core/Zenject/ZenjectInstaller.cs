using DoubleDrift.UIModule;
using Zenject;

namespace DoubleDrift
{
    public class ZenjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelSignals>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CarManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<InfinityPathManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SlideControl>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
