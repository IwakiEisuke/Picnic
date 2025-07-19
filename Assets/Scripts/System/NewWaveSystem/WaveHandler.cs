using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ウェーブを実行するクラス
/// </summary>
public class WaveHandler : MonoBehaviour
{
    [SerializeField] StageData stageData;

    Queue<RuntimeSpawnEvent> spawnEvents;
    int currentWaveIndex = 0;

    float waveTimer = 0;
    bool isWavePlaying = false;

    [ContextMenu("Start Wave")]
    public void StartWave()
    {
        if (stageData == null)
        {
            Debug.LogWarning($"{nameof(WaveHandler)}に{nameof(StageData)}がアサインされていないためウェーブを開始できません。");
            return;
        }

        spawnEvents = SpawnEventConverter.ConvertQueue(stageData.waves[currentWaveIndex]);
        currentWaveIndex++;
        isWavePlaying = true;
    }

    private void Update()
    {
        if (!isWavePlaying) return;

        waveTimer += Time.deltaTime;

        // 実行時間になったスポーンイベントを全て実行する
        while (spawnEvents.Count > 0)
        {
            var next = spawnEvents.Peek(); // 最も直近のスポーンイベントを取得

            // 実行時間か？
            if (next.time <= waveTimer)
            {
                ExecuteSpawnEvent(next); // スポーン処理を実行
                spawnEvents.Dequeue(); // 実行したスポーンイベントをキューから削除
            }
            else
            {
                // 直近のスポーンイベントが実行時間に達していない場合はループを抜ける
                return;
            }
        }

        // ウェーブが終了した場合の処理
        isWavePlaying = false;
    }

    private void ExecuteSpawnEvent(RuntimeSpawnEvent spawnEvent)
    {
        spawnEvent.spawnPoint.Spawn(spawnEvent);
    }
}

/// <summary>
/// スポーンイベントから実行時に処理しやすい形式に変換するクラス
/// </summary>
public static class SpawnEventConverter
{
    /// <summary>
    /// WaveDataからSpawnEvent配列を生成します
    /// </summary>
    /// <param name="wave"></param>
    /// <returns></returns>
    public static RuntimeSpawnEvent[] Convert(WaveData wave)
    {
        // 加工前のスポーンイベントを取得
        var originalSpawnEvents = wave.spawnEvents;

        // リピート処理をリピート回数分のSpawnEventに分けるため事前にカウントして配列の長さを決める
        var eventsCount = 0;
        for (int i = 0; i < originalSpawnEvents.Count; i++)
        {
            eventsCount += originalSpawnEvents[i].repeatCount;
        }

        var convertedSpawnEvents = new RuntimeSpawnEvent[eventsCount];
        var count = 0;

        for (int i = 0; i < originalSpawnEvents.Count; i++)
        {
            // 繰り返しイベントをリピート回数分のRuntimeSpawnEventに変換する
            for (int j = 0; j < originalSpawnEvents[i].repeatCount; j++)
            {
                convertedSpawnEvents[count] = new RuntimeSpawnEvent()
                {
                    time = originalSpawnEvents[i].time + originalSpawnEvents[i].repeatInterval * j,
                    spawnPoint = wave.SpawnPoints[originalSpawnEvents[i].spawnPointIndex],
                    spawnCount = originalSpawnEvents[i].spawnCountPerBatch,
                    entityData = wave.EntitiesData[originalSpawnEvents[i].enemyIndex]
                };
                count++;
            }
        }

        return convertedSpawnEvents;
    }

    /// <summary>
    /// 実行時間でソートされたスポーンイベントのキューを生成します
    /// </summary>
    /// <param name="wave"></param>
    /// <returns></returns>
    public static Queue<RuntimeSpawnEvent> ConvertQueue(WaveData wave)
    {
        return new Queue<RuntimeSpawnEvent>(Convert(wave).OrderBy(x => x.time));
    }
}

/// <summary>
/// 実行時に使用するスポーンイベント情報
/// </summary>
public class RuntimeSpawnEvent
{
    /// <summary>
    /// このスポーンイベントの実行時間（ウェーブ開始時からの経過時間）
    /// </summary>
    public float time;
    public SpawnPointBase spawnPoint;
    public int spawnCount;
    public SpawnEntityData entityData;
}