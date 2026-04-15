using UnityEngine;

namespace Bonbibi
{
    public class TriggerGroupEnabler : MonoBehaviour
    {
        [SerializeField] private DialogueTrigger[] triggersToEnable;

        public void EnableAll()
        {
            foreach (DialogueTrigger dialogueTrigger in triggersToEnable)
            {
                dialogueTrigger.gameObject.SetActive(true);
            }
        }
    }
}