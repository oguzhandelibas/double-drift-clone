using UnityEngine;

namespace DoubleDrift
{
    public class BootLoader : AbstractSingleton<BootLoader>
    {
        [SerializeField] private LevelPathData[] pathDatas;
        private void Awake()
        {
            GameObject gameObj = Object.Instantiate(Resources.Load<GameObject>($"Game"));
            gameObj.name = "--->GAME";
            gameObj.GetComponent<GameManager>().Initialize(pathDatas[0]);
        }
    }
}