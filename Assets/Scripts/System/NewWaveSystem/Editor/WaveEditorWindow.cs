using UnityEditor;
using UnityEngine;

public class WaveEditorWindow : EditorWindow
{
    private float secondsPerPixel = 0.1f;
    private float zoomMin = 0.01f;
    private float zoomMax = 1.0f;
    private float scrollOffsetX = 0f;
    private float timeLabelHeight = 20f;

    private EnemySpawnEvent selectedEvent = null;

    private EnemySpawnEvent draggingEvent = null;
    private Vector2 dragStartMousePos;
    private float dragStartTime;
    private int dragStartSpawnPointIndex;

    private EnemySpawnEvent candidateSelectEvent = null;
    private Vector2 mouseDownPos;
    private bool isDragging = false;
    private bool hasRecordedDragUndo = false;



    [MenuItem("Tools/Wave Editor")]
    public static void ShowWindow()
    {
        GetWindow<WaveEditorWindow>("Wave Editor");
    }

    public static void OpenWithWaveData(WaveData waveData)
    {
        var window = GetWindow<WaveEditorWindow>("Wave Editor");
        window.currentWaveData = waveData;
        window.Repaint();
    }

    private WaveData currentWaveData;

    private void OnGUI()
    {
        currentWaveData = (WaveData)EditorGUILayout.ObjectField("Wave Data", currentWaveData, typeof(WaveData), false);

        if (currentWaveData == null)
            return;

        HandleZoom();
        HandleScroll();

        GUILayout.Space(40);

        DrawTimeline(currentWaveData);
    }

    private void DrawTimeline(WaveData wave)
    {
        if (wave == null) return;

        float maxTime = 300f;
        float indexColumnWidth = 50f;
        float rowHeight = 40f;
        float timelineWidth = position.width - indexColumnWidth;
        float timelineHeight = wave.SpawnPoints.Count * rowHeight;
        float totalHeight = wave.SpawnPoints.Count * rowHeight;

        Rect totalRect = GUILayoutUtility.GetRect(position.width, wave.SpawnPoints.Count * rowHeight);
        Rect indexRect = new Rect(totalRect.x, totalRect.y, indexColumnWidth, totalRect.height);
        Rect timelineRect = new Rect(indexRect.xMax, totalRect.y, timelineWidth, totalRect.height);

        EditorGUI.DrawRect(indexRect, new Color(0.12f, 0.12f, 0.12f));

        for (int i = 0; i < wave.SpawnPoints.Count; i++)
        {
            float y = indexRect.y + i * rowHeight;
            Rect cell = new Rect(indexRect.x, y, indexRect.width, rowHeight);
            GUI.Label(new Rect(cell.x + 4, cell.y + 10, cell.width - 8, 20), i.ToString(), EditorStyles.boldLabel);
        }

        for (int i = 0; i < wave.SpawnPoints.Count; i++)
        {
            float y = timelineRect.y + i * rowHeight;
            Rect row = new Rect(timelineRect.x, y, timelineRect.width, rowHeight);
            EditorGUI.DrawRect(row, i % 2 == 0 ? new Color(0.15f, 0.15f, 0.15f) : new Color(0.2f, 0.2f, 0.2f));
        }

        Rect markerRectArea = new Rect(timelineRect.x, timelineRect.y, timelineRect.width, timelineRect.height);
        GUI.BeginGroup(new Rect(timelineRect.x, timelineRect.y - 20, timelineRect.width, timelineRect.height + timeLabelHeight));
        DrawTimeMarkers(new Rect(0, 20, markerRectArea.width, markerRectArea.height + timeLabelHeight), secondsPerPixel, maxTime, scrollOffsetX);
        GUI.EndGroup();

        GUI.BeginGroup(timelineRect);
        var localRect = new Rect(0, 0, timelineRect.width, timelineRect.height);

        // 対数スケールでサイズを決定（1〜100体で徐々に大きく）
        float GetSizeFromSpawnCount(int count, float minSize = 10f, float maxSize = 15f)
        {
            if (count <= 1) return minSize;
            float logValue = Mathf.Log10(count); // 1→0, 10→1, 100→2
            float t = Mathf.Clamp01(logValue / 2f); // 正規化: log10(1)〜log10(100)
            return Mathf.Lerp(minSize, maxSize, t);
        }

        foreach (var evt in wave.spawnEvents)
        {
            if (evt.spawnPointIndex < 0 || evt.spawnPointIndex >= wave.SpawnPoints.Count) continue;

            // 繰り返し回数分ループ
            for (int r = 0; r < evt.repeatCount; r++)
            {
                float eventTime = evt.time + r * evt.repeatInterval;
                float x = eventTime / secondsPerPixel - scrollOffsetX;
                float y = evt.spawnPointIndex * rowHeight + rowHeight / 2f; // 縦中央に配置

                // ダイヤの対角線の長さ
                float size = (r == 0) ? GetSizeFromSpawnCount(evt.spawnCountPerBatch) : 8f;
                

                Vector3 p1 = new Vector3(x, y - size / 2f);   // 上点
                Vector3 p2 = new Vector3(x + size / 2f, y);   // 右点
                Vector3 p3 = new Vector3(x, y + size / 2f);   // 下点
                Vector3 p4 = new Vector3(x - size / 2f, y);   // 左点

                Vector3[] diamondPoints = new Vector3[] { p1, p2, p3, p4 };

                if (x + size / 2f >= 0 && x - size / 2f <= localRect.width)
                {
                    Handles.color = Color.red;
                    Handles.DrawAAConvexPolygon(diamondPoints);

                    // スポーン数の数字を表示
                    GUIStyle style = new GUIStyle(EditorStyles.miniLabel)
                    {
                        normal = { textColor = Color.white },
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = (r == 0) ? 11 : 5
                    };

                    float labelWidth = 40f;
                    float labelHeight = (r == 0) ? 14f : 12f;

                    // ラベル位置（アイコンの上に表示）
                    Rect labelRect = new Rect(x - labelWidth / 2f, y - size / 2f - labelHeight - 2, labelWidth, labelHeight);
                    GUI.Label(labelRect, $"{evt.spawnCountPerBatch}", style);
                }
            }

            // 繰り返しイベントの線を描画
            if (evt.repeatCount > 0 && evt.repeatInterval > 0f)
            {
                float startX = evt.time / secondsPerPixel - scrollOffsetX;
                float endTime = evt.time + evt.repeatInterval * (evt.repeatCount - 1);
                float endX = endTime / secondsPerPixel - scrollOffsetX;
                float centerY = evt.spawnPointIndex * rowHeight + rowHeight / 2f;

                Handles.color = new Color(1, 0, 0, 0.5f);
                Handles.DrawLine(new Vector3(startX, centerY, 0), new Vector3(endX, centerY, 0));
                Handles.DrawLine(new Vector3(startX, centerY - 1, 0), new Vector3(endX, centerY - 1, 0));
                Handles.DrawLine(new Vector3(startX, centerY + 1, 0), new Vector3(endX, centerY + 1, 0));
            }
        }

        GUI.EndGroup();


        Event e = Event.current;

        if (e.type == EventType.ContextClick && timelineRect.Contains(e.mousePosition))
        {
            float time = (e.mousePosition.x - timelineRect.x + scrollOffsetX) * secondsPerPixel;
            int rowIndex = (int)((e.mousePosition.y - timelineRect.y) / rowHeight);
            ShowContextMenuAt(time, rowIndex);
        }

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0 && timelineRect.Contains(e.mousePosition))
                {
                    float clickTime = (e.mousePosition.x - timelineRect.x + scrollOffsetX) * secondsPerPixel;
                    int clickRow = (int)((e.mousePosition.y - timelineRect.y) / 40f);

                    candidateSelectEvent = null;
                    float maxClickTimeDiff = secondsPerPixel * 10f;

                    foreach (var evt in currentWaveData.spawnEvents)
                    {
                        if (evt.spawnPointIndex == clickRow && Mathf.Abs(evt.time - clickTime) < maxClickTimeDiff)
                        {
                            candidateSelectEvent = evt;
                            mouseDownPos = e.mousePosition;
                            isDragging = false;
                            break;
                        }
                    }

                    e.Use();
                }
                break;

            case EventType.MouseDrag:
                if (candidateSelectEvent != null)
                {
                    if (!isDragging)
                    {
                        // 移動距離が閾値を超えたらドラッグ開始
                        if (Vector2.Distance(e.mousePosition, mouseDownPos) > 4f)
                        {
                            isDragging = true;

                            // ドラッグ用の開始データをセット
                            draggingEvent = candidateSelectEvent;
                            dragStartMousePos = mouseDownPos;
                            dragStartTime = draggingEvent.time;
                            dragStartSpawnPointIndex = draggingEvent.spawnPointIndex;

                            selectedEvent = draggingEvent; // ドラッグ開始時に選択状態にする
                        }
                    }

                    if (isDragging && draggingEvent != null)
                    {
                        if (!hasRecordedDragUndo)
                        {
                            Undo.RecordObject(currentWaveData, "Move Spawn Event");
                            hasRecordedDragUndo = true;
                        }

                        Vector2 delta = e.mousePosition - dragStartMousePos;

                        float newTime = dragStartTime + delta.x * secondsPerPixel;

                        // Ctrl押下中は1秒単位にスナップ
                        if (e.control)
                        {
                            newTime = Mathf.Round(newTime); // 1秒単位でスナップ
                        }

                        draggingEvent.time = Mathf.Max(0, newTime);
                        draggingEvent.spawnPointIndex = Mathf.Clamp(dragStartSpawnPointIndex + Mathf.RoundToInt(delta.y / 40f), 0, currentWaveData.SpawnPoints.Count - 1);

                        EditorUtility.SetDirty(currentWaveData);
                        Repaint();

                        e.Use();
                    }
                }
                break;

            case EventType.MouseUp:
                if (candidateSelectEvent != null)
                {
                    if (!isDragging)
                    {
                        // 移動が閾値以下なのでクリックと判断して選択確定
                        selectedEvent = candidateSelectEvent;
                        Repaint();
                    }

                    candidateSelectEvent = null;
                    draggingEvent = null;
                    isDragging = false;

                    e.Use();
                }
                break;
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-", GUILayout.Width(30))) secondsPerPixel = Mathf.Clamp(secondsPerPixel + 0.01f, zoomMin, zoomMax);
        if (GUILayout.Button("+", GUILayout.Width(30))) secondsPerPixel = Mathf.Clamp(secondsPerPixel - 0.01f, zoomMin, zoomMax);
        GUILayout.Label($"Zoom: {secondsPerPixel:0.000}s/pixel");
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (selectedEvent != null)
        {
            // 編集を監視
            EditorGUI.BeginChangeCheck();

            Undo.RecordObject(currentWaveData, "Edit Spawn Event");

            EditorGUILayout.LabelField("Selected Spawn Event Details", EditorStyles.boldLabel);

            // スポーンポイントは選択肢が限られるならPopupなどで編集可能に
            selectedEvent.spawnPointIndex = EditorGUILayout.IntSlider("Spawn Point Index", selectedEvent.spawnPointIndex, 0, currentWaveData.SpawnPoints.Count - 1);

            // 時間はfloatで入力
            selectedEvent.time = EditorGUILayout.FloatField("Time (s)", selectedEvent.time);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Spawn Settings", EditorStyles.boldLabel);

            // 敵はenemyPrefabsから選択できるようにPopup
            if (currentWaveData.EnemyPrefabs != null && currentWaveData.EnemyPrefabs.Count > 0)
            {
                string[] enemyNames = new string[currentWaveData.EnemyPrefabs.Count];
                for (int i = 0; i < enemyNames.Length; i++)
                {
                    enemyNames[i] = currentWaveData.EnemyPrefabs[i]?.name ?? "Null";
                }

                int newEnemyIndex = EditorGUILayout.Popup("Enemy", selectedEvent.enemyIndex, enemyNames);
                selectedEvent.enemyIndex = newEnemyIndex;
            }
            else
            {
                EditorGUILayout.LabelField("Enemy prefabs not assigned.");
            }

            // 一度にスポーンさせる量
            selectedEvent.spawnCountPerBatch = EditorGUILayout.IntField("Spawn Count Per Batch", selectedEvent.spawnCountPerBatch);
            selectedEvent.spawnCountPerBatch = Mathf.Max(1, selectedEvent.spawnCountPerBatch);

            // スポーンの繰り返し回数
            selectedEvent.repeatCount = EditorGUILayout.IntField("Repeat Count", selectedEvent.repeatCount);
            selectedEvent.repeatCount = Mathf.Max(1, selectedEvent.repeatCount);

            // 繰り返し間隔（秒）
            selectedEvent.repeatInterval = EditorGUILayout.FloatField("Repeat Interval (s)", selectedEvent.repeatInterval);
            selectedEvent.repeatInterval = Mathf.Max(0f, selectedEvent.repeatInterval);

            // 編集したらデータをDirtyにする（保存フラグ）
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(currentWaveData);
            }
        }
        else
        {
            EditorGUILayout.LabelField("No spawn event selected.");
        }
    }

    private void ShowContextMenuAt(float time, int spawnPointIndex)
    {
        var menu = new GenericMenu();

        if (currentWaveData == null || spawnPointIndex >= currentWaveData.SpawnPoints.Count)
            return;

        // 追加：右クリック位置付近のイベントを検索（時間とスポーンポイントが一致）
        EnemySpawnEvent eventToDelete = null;
        float maxTimeDiff = secondsPerPixel * 10f; // クリック許容範囲（ピクセル→時間換算）
        foreach (var evt in currentWaveData.spawnEvents)
        {
            if (evt.spawnPointIndex == spawnPointIndex && Mathf.Abs(evt.time - time) < maxTimeDiff)
            {
                eventToDelete = evt;
                break;
            }
        }

        // もしイベントがあれば削除メニューを追加
        if (eventToDelete != null)
        {
            menu.AddItem(new GUIContent("Delete Spawn Event"), false, () =>
            {
                Undo.RecordObject(currentWaveData, "Delete Spawn Event");

                // 選択解除
                if (selectedEvent == eventToDelete) selectedEvent = null;
                currentWaveData.spawnEvents.Remove(eventToDelete);
                EditorUtility.SetDirty(currentWaveData);

                GUI.changed = true;
                Repaint();
            });
        }
        else
        {
            menu.AddDisabledItem(new GUIContent("Delete Spawn Event"));
        }

        // 既存の追加メニュー
        for (int i = 0; i < currentWaveData.EnemyPrefabs.Count; i++)
        {
            string enemyName = currentWaveData.EnemyPrefabs[i]?.name ?? "Null";
            int ei = i;
            menu.AddItem(new GUIContent($"Add {enemyName}"), false, () =>
            {
                Undo.RecordObject(currentWaveData, "Add Spawn Event");

                var evt = new EnemySpawnEvent
                {
                    time = time,
                    enemyIndex = ei,
                    spawnPointIndex = spawnPointIndex
                };
                currentWaveData.spawnEvents.Add(evt);
                EditorUtility.SetDirty(currentWaveData);
            });
        }

        if (menu.GetItemCount() == 0)
        {
            menu.AddDisabledItem(new GUIContent("No enemy prefabs"));
        }

        menu.ShowAsContext();
    }


    private void DrawTimeMarkers(Rect timelineRect, float secondsPerPixel, float maxTime, float scrollOffsetX)
    {
        float startX = -scrollOffsetX;
        float endX = timelineRect.width;

        float zoomLevel = 1f / secondsPerPixel;

        float minorInterval;
        float majorInterval;

        if (zoomLevel >= 100f)
        {
            minorInterval = 0.1f;
            majorInterval = 1f;
        }
        else if (zoomLevel >= 50f)
        {
            minorInterval = 0.2f;
            majorInterval = 1f;
        }
        else if (zoomLevel >= 20f)
        {
            minorInterval = 0.4f;
            majorInterval = 2f;
        }
        else if (zoomLevel >= 10f)
        {
            minorInterval = 1f;
            majorInterval = 5f;
        }
        else
        {
            minorInterval = 2f;
            majorInterval = 10f;
        }

        Handles.BeginGUI();

        Handles.color = new Color(1f, 1f, 1f, 0.05f);
        for (float t = 0; t < maxTime; t += minorInterval)
        {
            float x = startX + t / secondsPerPixel;
            if (x > endX) break;

            Handles.DrawLine(new Vector2(x, 20), new Vector2(x, timelineRect.height));
        }

        Handles.color = new Color(1f, 1f, 1f, 0.15f);
        for (float t = 0; t <= maxTime; t += majorInterval)
        {
            float x = startX + t / secondsPerPixel;
            if (x > endX) break;

            Handles.DrawLine(new Vector2(x, 20), new Vector2(x, timelineRect.height));
            GUI.Label(new Rect(x + 2, 0, 60, timeLabelHeight), $"{t:0.##}s", EditorStyles.miniBoldLabel);
        }

        Handles.EndGUI();
    }

    private void HandleZoom()
    {
        Event e = Event.current;
        if (e.type == EventType.ScrollWheel && e.control)
        {
            float oldSecondsPerPixel = secondsPerPixel;

            // マウス位置のタイムライン上のローカルX（スクロール込み）
            Vector2 mousePos = e.mousePosition;

            // timelineRect のX座標と幅はDrawTimelineのindexColumnWidthと位置に依存するので、ここでは固定値で合わせる
            float timelineX = 50f; // indexColumnWidthに合わせて調整してください
            float timelineWidth = position.width - timelineX;

            // マウスの位置（ローカルタイムライン座標）
            float mouseLocalX = mousePos.x - timelineX;

            if (mouseLocalX < 0 || mouseLocalX > timelineWidth)
                return; // タイムライン外ならズームしない

            // マウスが指している時間（ズーム前）
            float timeAtMouse = (mouseLocalX + scrollOffsetX) * oldSecondsPerPixel;

            // ズームの更新
            float zoomFactor = 0.1f;
            if (e.delta.y > 0)
            {
                secondsPerPixel *= 1 + zoomFactor;
            }
            else if (e.delta.y < 0)
            {
                secondsPerPixel *= 1 - zoomFactor;
            }
            secondsPerPixel = Mathf.Clamp(secondsPerPixel, zoomMin, zoomMax);

            // ズーム後にマウス位置の時間が同じになるようscrollOffsetXを調整
            scrollOffsetX = timeAtMouse / secondsPerPixel - mouseLocalX;

            // scrollOffsetXは負にならないよう制限
            scrollOffsetX = Mathf.Max(scrollOffsetX, 0f);

            e.Use();
            Repaint();
        }
    }


    private void HandleScroll()
    {
        Event e = Event.current;
        if (e.type == EventType.ScrollWheel && e.shift)
        {
            float scrollSpeed = 20f;
            scrollOffsetX += e.delta.x * scrollSpeed;
            scrollOffsetX = Mathf.Max(scrollOffsetX, 0f);
            e.Use();
            Repaint();
        }
    }
}
