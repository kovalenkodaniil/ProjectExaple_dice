using PlayerScripts;
using TMPro;

public class TalentsViewExpObserver
{
    private bool isEnable;
    private readonly TMP_Text lb;
    private readonly Player player;

    public TalentsViewExpObserver(TMP_Text lb, Player player)
    {
        this.lb = lb;
        this.player = player;
    }

    public void Enable()
    {
        if (isEnable)
            return;
        isEnable = true;
        player.model.OnExpChanged += HandlerOnExpChanged;
        View_UpdateExp();
    }

    public void Disable()
    {
        if (!isEnable)
            return;
        isEnable = false;
        player.model.OnExpChanged -= HandlerOnExpChanged;
    }

    private void HandlerOnExpChanged()
    {
        View_UpdateExp();
    }

    private void View_UpdateExp()
    {
        lb.text = $"{Managers.Localization.Translate(LocalizationKeys.com_exp)} <size=130%>{player.model.Expirience}</size>";
    }
}