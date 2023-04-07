using SpaceBattle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

internal class Program
{
    private static void Main(string[] args)
    {
        string battleLogFileName = $"[{DateTime.Now:yyyy-MM-dd}] battle log.txt";
        TextWriter battleLooger = new StreamWriter(battleLogFileName);

        List<Bonus> bonusDatabase = new()
        {
            new Bonus (type: BonusType.Shield, value: 50, isMultiplicative: false),
            new Bonus (type: BonusType.Armor, value: 50, isMultiplicative: false),
            new Bonus (type: BonusType.ReloadTime, value: -0.2, isMultiplicative: true),
            new Bonus(type: BonusType.ShieldRepaired, value: 0.2, isMultiplicative: true)
        };

        List<Armor> armorDatabase = new()
        {
            new Armor(name: "Base Armor", type: BonusType.Armor, armor: 100),
            new Armor(name: "Base Armor", type: BonusType.Armor, armor: 60)
        };

        List<Shield> shieldDatabase = new()
        {
            new Shield(name: "Base Shield", capacityType: BonusType.Shield, capacity: 80,
            reloadType: BonusType.ReloadTime, reloadTime: 1, reloadAmountType: BonusType.ShieldRepaired, reloadAmount: 1),
            new Shield(name: "Base Shield", capacityType: BonusType.Shield, capacity: 120,
            reloadType: BonusType.ReloadTime, reloadTime: 1, reloadAmountType: BonusType.ShieldRepaired, reloadAmount: 1)
        };

        List<Module> moduleDatabase = new()
        {
            new Module("Base Shield", new Bonus (type: BonusType.Shield, value: 50, isMultiplicative: false)),
            new Module("Base Armor", new Bonus (type: BonusType.Armor, value: 50, isMultiplicative: false)),
            new Module("Base Recharger", new Bonus (type: BonusType.ReloadTime, value: -0.2, isMultiplicative: true)),
            new Module("Base Shield Recharger", new Bonus(type: BonusType.ShieldRepaired, value: 0.2, isMultiplicative: true))
        };

        List<Ordnance> gunDatabase = new()
        {
            new Ordnance(name: "Base Gun A", damageType: BonusType.GunDamage, damage: 5, reloadType: BonusType.ReloadTime, reloadTime: 3),
            new Ordnance(name: "Base Gun B", damageType: BonusType.GunDamage, damage: 4, reloadType: BonusType.ReloadTime, reloadTime: 2),
            new Ordnance(name: "Base Gun C", damageType: BonusType.GunDamage, damage: 20, reloadType: BonusType.ReloadTime, reloadTime: 5)
        };

        Ship shipA = new(name: "Ship A", armor: 100, shield: 80, shieldRechargeTime: 1,
            shieldRechargeAmount: 1, gunPorts: 2, modulePorts: 2, gunDatabase, moduleDatabase);

        Ship shipB = new(name: "Ship B", armor: 60, shield: 120, shieldRechargeTime: 1,
            shieldRechargeAmount: 1, gunPorts: 2, modulePorts: 3, gunDatabase, moduleDatabase);

        shipA.GetShipStats();
        shipB.GetShipStats();

        shipA.ApplyBonuses(isPrinted: true);
        shipB.ApplyBonuses(isPrinted: true);

        List<Ship> shipList = new()
        {
            shipA,
            shipB
        };

        //List<Ship> shipList = new();
        //for (int i = 0; i < 150; i++)
        //{
        //    Ship ship = new($"Ship {i}");
        //    ship.ApplyBonuses(isPrinted: false);
        //    shipList.Add(ship);
        //}

        Instance instance = new(shipList, battleLooger, isBattleLogPrintedInConsole: false);
        Stopwatch stopwatch = new();
        stopwatch.Start();
        instance.Start();
        stopwatch.Stop();
        TimeSpan timeSpan = stopwatch.Elapsed;
        Console.WriteLine($"Execution time: {timeSpan.Minutes}M:{timeSpan.Seconds}S:{timeSpan.Milliseconds}MS");
        Console.WriteLine($"\"{battleLogFileName}\" detailed battle log is available in the program folder.");
        Console.ReadLine();
    }
}