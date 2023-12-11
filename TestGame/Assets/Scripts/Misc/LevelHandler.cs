using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public SpawnTransformData spawnTransformData;
    public GameObject character;
    public GameTimer gameTimer;
    public GameObject victoryTextObj;

    private struct SpawnData
    {
        public Vector3 currentSpawnPosition;
        public Quaternion currentSpawnRotation;
    }
    private SpawnData currentSpawnData;

    private void Start()
    {
        currentSpawnData = new SpawnData();
        SetSpawnPoint(0);
    }

    private void OnEnable()
    {
        TriggerEvent.onTrigger += TriggerEvent_onTrigger;
    }

    private void OnDisable()
    {
        TriggerEvent.onTrigger -= TriggerEvent_onTrigger;
    }

    private void TriggerEvent_onTrigger(TriggerType triggerType)
    {
        HandleTriggerType(triggerType);
    }

    //setting condition based on trigger that is collided
    private void HandleTriggerType(TriggerType triggerType)
    {
        switch (triggerType)
        {
            case TriggerType.FallTrigger:
                SpawnToCurrentSpawnPoint();
                break;
            case TriggerType.VictoryTrigger:
                RestartGame();
                break;
            case TriggerType.SpawnPointOneTrigger:
                SetSpawnPoint(0);
                break;
            case TriggerType.SpawnPointTwoTrigger:
                SetSpawnPoint(1);
                break;
                case TriggerType.SpawnPointThreeTrigger:
                SetSpawnPoint(2);
                break;
        }
    }

    private void SpawnToCurrentSpawnPoint()
    {
        character.transform.position = currentSpawnData.currentSpawnPosition;
        character.transform.rotation = currentSpawnData.currentSpawnRotation;
    }

    private void RestartGame()
    {
        character.gameObject.SetActive(false);
        gameTimer.StopTimer();
        victoryTextObj.SetActive(true);
        Invoke(nameof(ReloadLevel), 2f);
    }

    private void SetSpawnPoint(int currentPoint)
    {
        currentSpawnData.currentSpawnPosition = spawnTransformData.spawnTransforms[currentPoint].position;
        currentSpawnData.currentSpawnRotation = spawnTransformData.spawnTransforms[currentPoint].rotation;
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }
}
