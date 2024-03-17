using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Profiling;
using UnityEngine;

public class RAM_Collector : MonoBehaviour
{

    string statsText;
    private string filePath = "C:\\Users\\separk\\Desktop\\ugrp\\data_auto\\final\\cpu_ram_reduced_result.txt"; // 파일 경로를 적절히 지정하세요.
    ProfilerRecorder totalReservedMemoryRecorder;
    ProfilerRecorder gcReservedMemoryRecorder;
    ProfilerRecorder systemUsedMemoryRecorder;
    ProfilerRecorder TotalUsedMemoryRecorder;

    ProfilerRecorder renderRecorder;
    ProfilerRecorder physicsRecorder;
    ProfilerRecorder scriptRecorder;
    ProfilerRecorder mainThreadTimeRecorder;


    void OnEnable()
    {
        totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        //gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        TotalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        TotalUsedMemoryRecorder.Start();
        renderRecorder = new ProfilerRecorder(ProfilerCategory.Render, "Rendering Time");
        renderRecorder.Start();
        physicsRecorder = new ProfilerRecorder(ProfilerCategory.Physics, "Physics Time");
        physicsRecorder.Start();
        mainThreadTimeRecorder = new ProfilerRecorder(ProfilerCategory.Internal, "Main Thread", 15);
        mainThreadTimeRecorder.Start();
        //scriptRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Scripts, "Physics Time");

    }

    void OnDisable()
    {
        totalReservedMemoryRecorder.Dispose();
        gcReservedMemoryRecorder.Dispose();
        systemUsedMemoryRecorder.Dispose();

        renderRecorder.Dispose();
        physicsRecorder.Dispose();
        scriptRecorder.Dispose();
        mainThreadTimeRecorder.Dispose();
    }



    void Update()
    {
        var sb = new StringBuilder(500);
        sb.AppendLine($"Record Time: {Time.realtimeSinceStartup} sec");
        if (totalReservedMemoryRecorder.Valid)
            sb.AppendLine($"Total Reserved Memory: {totalReservedMemoryRecorder.LastValue / (1024 * 1024)} MB");
        if (TotalUsedMemoryRecorder.Valid)
            sb.AppendLine($"Total Used Memory: {TotalUsedMemoryRecorder.LastValue / (1024 * 1024)} MB");
        if (systemUsedMemoryRecorder.Valid)
            sb.AppendLine($"System Used Memory: {systemUsedMemoryRecorder.LastValue / (1024 * 1024)} MB");
        //sb.AppendLine($"CPU rendering time: {renderRecorder.LastValue}");
        //sb.AppendLine($"CPU Physics time: {physicsRecorder.LastValue}");
        //sb.AppendLine($"Rendering Time: {mainThreadTimeRecorder.LastValue * (1e-6f):F1} ms");
        sb.AppendLine($"CPU mainthread time: {mainThreadTimeRecorder.LastValue * (1e-6f):F1} ms");
        //sb.AppendLine($"CPU Scripts time: {scriptRecorder.LastValue}");

        SaveDataToFile(sb.ToString());
    }

    void SaveDataToFile(string data)
    {
        try
        {
            // 파일이 없으면 생성하고 있으면 기존 내용에 추가
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data to file: {e.Message}");
        }
    }

}
