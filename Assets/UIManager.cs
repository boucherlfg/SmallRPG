using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
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
}
