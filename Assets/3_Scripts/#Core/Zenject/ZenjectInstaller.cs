using DoubleDrift.UIModule;
using Zenject;

namespace DoubleDrift
{
    public class ZenjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
