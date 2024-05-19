using System.Threading.Tasks;
using UnityEngine;

namespace DoubleDrift
{
    public class BootLoader : AbstractSingleton<BootLoader>
    {
        [SerializeField] private LevelPathData[] pathDatas;
        private GameObject _gameObject;

        public int GetLevelDataCount() => pathDatas.Length;
        public LevelPathData GetLevelPath(int index) => pathDatas[index];

        protected override void Awake()
        {
            base.Awake();
            Initialize(true);
        }

        public async void Initialize(bool firstInit = false)
        {
            await Task.Delay(100);
            if(_gameObject != null) Destroy(_gameObject);
            _gameObject = Object.Instantiate(Resources.Load<GameObject>($"Game"));
            _gameObject.name = "--->GAME";
            _gameObject.GetComponent<GameManager>().Initialize(pathDatas[0], true);
        }
    }
}