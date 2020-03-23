using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController controller;

    public void SetController(PlayerController controller)
    {
        this.controller = controller;
    }

    private void Update()
    {
        SetFuelAmount(controller.GetThrusterFuelAmount());
    }

    void SetFuelAmount(float amount)
    {
        thrusterFuelFill.localScale = new Vector3(1.0f, amount, 1.0f);
    }
}
