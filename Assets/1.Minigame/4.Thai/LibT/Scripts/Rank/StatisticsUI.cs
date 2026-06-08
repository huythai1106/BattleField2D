using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsUI : MonoBehaviour
{
    public virtual void Show(StatPlayer stat) { }

    public virtual void Show() { }

    public virtual void Hidden()
    {
        gameObject.SetActive(false);
    }
}
