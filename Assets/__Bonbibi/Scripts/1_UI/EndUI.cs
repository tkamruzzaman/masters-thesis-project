using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button creditsButton;


    private void Awake()
    {
        menuButton.onClick.AddListener(() =>
        {
            GameServices.Instance.navigationManager.LoadScene(Scenes.Menu);
        });

        creditsButton.onClick.AddListener(() => { });
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
