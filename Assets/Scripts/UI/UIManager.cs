using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public static PanelWrapper[] Panels => _instance.GetComponentsInChildren<PanelWrapper>();
    [SerializeField]
    private PausePanel pause;
    public static PausePanel Pause => _instance.pause;
    [SerializeField]
    private NotifManager notifications;
    public static NotifManager Notifications => _instance.notifications;

    [SerializeField]
    private GameOverPanel gameOver;
    public static GameOverPanel GameOver => _instance.gameOver;

    [SerializeField]
    private InventoryPanel inventory;
    public static InventoryPanel Inventory => _instance.inventory;

    [SerializeField]
    private EquipmentPanel equipment;
    public static EquipmentPanel Equipment => _instance.equipment;

    [SerializeField]
    private StatsPanel stats;
    public static StatsPanel Stats => _instance.stats;

    [SerializeField]
    private CraftingPanel crafting;
    public static CraftingPanel Crafting => _instance.crafting;

    [SerializeField]
    private CreditsPanel credits;
    public static CreditsPanel Credits => _instance.credits;

    [SerializeField]
    private SettingsPanel settings;
    public static SettingsPanel Settings => _instance.settings;

    [SerializeField]
    private StorylinePanel storyline;
    public static StorylinePanel Storyline => _instance.storyline;

    [SerializeField]
    private SplashScreen splashScreen;
    public static SplashScreen SplashScreen => _instance.splashScreen;

    [SerializeField]
    private HowToPanel howTo;
    public static HowToPanel HowTo => _instance.howTo;

    [SerializeField]
    private LogPanel logs;
    public static LogPanel Logs => _instance.logs;

    [SerializeField]
    private TooltipPanel tooltip;
    public static TooltipPanel Tooltip => _instance.tooltip;
}
