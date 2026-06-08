
using UnityEngine;

namespace Minigame.Battlefield
{
    [System.Serializable]
    public class BaseModule
    {
        public BaseCharacter character;

        public BaseModule(BaseCharacter character)
        {
            this.character = character;
            Initialize();
        }

        protected virtual void Initialize() { }
    }
}
