using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;

    [SerializeField]
    private GameObject pauseMenu;

    private PlayerController controller;

    public void SetController(PlayerController controller)
    {
        this.controller = controller;
    }

    private void Start()
    {
        PauseMenu.isOn = false;
    }

    private void Update()
    {
        SetFuelAmount(controller.GetThrusterFuelAmount());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;

    }

    void SetFuelAmount(float amount)
    {
        thrusterFuelFill.localScale = new Vector3(1.0f, amount, 1.0f);
    }
}
