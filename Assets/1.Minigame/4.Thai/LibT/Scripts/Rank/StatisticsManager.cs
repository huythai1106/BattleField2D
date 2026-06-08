using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;
    public RankSetting rankSetting;
    public StatisticsUI[] statisticsUIs;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void HideStat()
    {
        foreach (var item in statisticsUIs)
        {
            item.Hidden();
        }
    }
}
