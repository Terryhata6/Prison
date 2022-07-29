using Game.Data;
using Game.Hero;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services.PersistantProgress;
using UnityEngine;

namespace Game.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        public string ProgressKey = "Progress";
        private readonly IPersistantProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistantProgressService progressService,IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);
            
            PlayerPrefs.SetString(ProgressKey, value:_progressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(ProgressKey)?
                .ToDeserialized<PlayerProgress>();
    }
}