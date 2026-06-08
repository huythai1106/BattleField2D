
using DG.Tweening;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class FighterAircrafeCrl : MonoBehaviour
    {
        public FighterAircraft fighterAircraft;
        public Transform start;
        public Transform end;
        public float timeFly;
        public float radius;
        public TeamType teamType;

        public void Init(TeamType teamType)
        {
            this.teamType = teamType;

            transform.SetPositionAndRotation(GetRandomPointPlane(), Quaternion.Euler(0, 0, Random.Range(-30, 30)));

            if (teamType == TeamType.Blue)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 180);
            }

            fighterAircraft.transform.position = start.position;
            fighterAircraft.teamType = teamType;
            fighterAircraft.transform.DOMove(end.position, 20f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public Vector3 GetRandomPointPlane()
        {
            Vector3 randomPoint = Random.insideUnitSphere * radius;
            randomPoint.z = 0;

            return randomPoint;
        }
    }
}