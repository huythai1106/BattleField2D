using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.Battlefield
{
    public class Base : MonoBehaviour
    {
        public BaseController baseController;
        public Team team;
        public SpriteRenderer flag;
        public Base[] baseConnect;
        private Coroutine progressCouroutine;

        public List<BaseCharacter> characterInBase = new List<BaseCharacter>();
        internal BaseCharacter characterInProgress;
        public Image progressImg;
        public SkeletonAnimation flagAnim;

        public float timeToCapture = 2f;

        public IEnumerator ChangeProgress(BaseCharacter c)
        {
            if (team == c.team) yield break;
            else
            {
                StartProgressUI();
                yield return new WaitForSeconds(timeToCapture);
                Succcess(c.team);
            }
        }

        private void StartProgressUI()
        {
            progressImg.gameObject.SetActive(true);
            progressImg.fillAmount = 1;
            progressImg.DOFillAmount(0, timeToCapture).SetEase(Ease.Linear);
        }

        public void StartProgress()
        {
            if (characterInBase.Count == 0)
            {
                if (progressCouroutine != null)
                {
                    StopCoroutine(progressCouroutine);
                    progressCouroutine = null;
                }
                StopProgress();
                characterInProgress = null;
                return;
            }

            if (!characterInProgress)
            {
                characterInProgress = characterInBase[0];
                progressCouroutine = StartCoroutine(ChangeProgress(characterInProgress));
            }
            else
            {
                var c = characterInBase[0];
                if (c.team == characterInProgress.team)
                {
                    return;
                }
                else
                {
                    if (progressCouroutine != null)
                    {
                        StopCoroutine(progressCouroutine);
                        progressCouroutine = null;
                    }
                    StopProgress();
                    characterInProgress = c;
                    progressCouroutine = StartCoroutine(ChangeProgress(characterInProgress));
                }
            }
        }

        public void StopProgress()
        {
            progressImg.gameObject.SetActive(false);
            progressImg.fillAmount = 1;
            DOTween.Kill(progressImg);
        }

        public void Succcess(Team team)
        {
            if (GameManager.Instance.isFinishGame) return;

            this.team = team;
            if (team.teamType == TeamType.Red)
            {
                baseController.baseRed.Add(this);
                baseController.baseBlue.Remove(this);
            }
            else
            {
                baseController.baseBlue.Add(this);
                baseController.baseRed.Remove(this);
            }

            StopProgress();
            SetFlag();
            baseController.SetupTarget();
            baseController.OnBaseChange(this);
            characterInBase.Clear();
        }

        public void OnPlayerDead()
        {

        }

        public void SetFlag()
        {
            if (team == null)
            {
                flag.sprite = baseController.flags[2];
                flagAnim.skeleton.SetSkin("White");
                flagAnim.skeleton.SetSlotsToSetupPose();
            }
            else
            {
                flag.sprite = baseController.flags[(int)team.teamType];
                flagAnim.skeleton.SetSkin(team.teamType.ToString());
                flagAnim.skeleton.SetSlotsToSetupPose();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Common.PLAYER_TAG))
            {
                var p = collision.GetComponent<BaseCharacter>();

                if (!p || p.team == team) return;

                characterInBase.Add(p);
                StartProgress();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Common.PLAYER_TAG))
            {
                var p = collision.GetComponent<BaseCharacter>();

                if (!p || p.team == team) return;

                characterInBase.Remove(p);
                StartProgress();
            }
        }
    }
}
