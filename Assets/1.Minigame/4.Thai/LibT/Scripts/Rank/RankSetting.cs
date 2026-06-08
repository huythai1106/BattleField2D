using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RankToImage
{
    public RankPlayer rankPlayer;
    public Sprite rankImage;
}

[CreateAssetMenu(menuName = "2Player/RankSetting")]
public class RankSetting : ScriptableObject
{
    public RankToImage[] rankToImages;

    public Sprite GetImage(RankPlayer r)
    {
        var rank = Array.Find(rankToImages, x => x.rankPlayer == r);

        return rank?.rankImage;
    }
}
