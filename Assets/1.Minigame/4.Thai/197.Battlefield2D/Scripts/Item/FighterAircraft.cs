using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class FighterAircraft : MonoBehaviour
    {
        public Rocket rocketPrefab;
        public float timeIntervalSpawnRocket;
        public TeamType teamType;

        private void Start()
        {
            InvokeRepeating(nameof(SpawnRocket), 1f, timeIntervalSpawnRocket);
        }

        private void SpawnRocket()
        {
            var obj = Instantiate(rocketPrefab);
            obj.transform.position = transform.position;
            obj.teamType = teamType;
        }
    }
}
