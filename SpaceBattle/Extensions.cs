using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SpaceBattle
{
    public static class Extensions
    {
        public static int GetUserChoice<Factory>(this List<Factory> collection)
        {
            int userChose;
            while (true)
            {
                Console.Write("\nPlease make your choice by specifying the index or type 0 if you want to leave the slot empy:\t");
                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out userChose) == false
                    || !(userChose > 0 && userChose - 1 < collection.Count)
                    && userChose != 0)
                {
                    Console.WriteLine("Invalid input, please try again");
                }
                else
                {
                    break;
                }
            }
            Console.Clear();

            return userChose - 1;
        }
        public static void PrintCollection<Factory>(this List<Factory> collection, int totalPorts, int selectedPorts, string name)
        {
            Console.WriteLine($"Constructing: {name}\n" +
                $"{typeof(Factory).Name} ports: {selectedPorts}/{totalPorts}. Select a {typeof(Factory).Name}:");
            if (collection.Count > 0)
            {
                int i = 1;

                foreach (var className in collection)
                {
                    Console.WriteLine($"{i}. {className}");
                    i++;
                }
            }
        }
        public static void GetShipStats(this Ship ship)
        {
            Console.WriteLine($"{nameof(Ship)} \"{ship.Name}\"\n");
            Console.WriteLine($"{nameof(ship.ArmorDeck)}:");
            ship.ArmorDeck.ForEach(x => Console.WriteLine($"\t{x}"));
            Console.WriteLine($"{nameof(ship.ShieldDeck)}:");
            ship.ShieldDeck.ForEach(x => Console.WriteLine($"\t{x}"));
            Console.WriteLine($"{nameof(ship.GunDeck)}:");
            ship.GunDeck.ForEach(x => Console.WriteLine($"\t{x}"));
            Console.WriteLine($"{nameof(ship.ModuleDeck)}:");
            ship.ModuleDeck.ForEach(x => Console.WriteLine($"\t{x}"));
            Console.WriteLine();
        }
        public static string PrintBonuses<T>(this List<T> bonuses)
        {
            string bonusValue = string.Empty;

            foreach (var bonus in bonuses)
            {
                if (bonus != null)
                {
                    bonusValue += bonus.ToString();
                }
            }
            return bonusValue;
        }
        public static List<T> GetItemsFromDatabase<T>(this List<T> database)
        {
            string jsonString = JsonSerializer.Serialize(database);
            return JsonSerializer.Deserialize<List<T>>(jsonString)!;
        }
        public static double GetValue(this Rechargeable rechargable)
            => rechargable.Bonuses
            .Where(x => x.Type == BonusType.GunDamage || x.Type == BonusType.ShieldRepaired)
            .Select(x => x.Value)
            .Sum();
        public static double GetReloadTime(this Rechargeable rechargable)
            => TimeSpan
            .FromSeconds(rechargable.Bonuses.Where(x => x.Type == BonusType.ReloadTime).Select(x => x.Value).Sum()).TotalMilliseconds;
        public static double ReleaseCharge(this Rechargeable rechargable, double battleTime)
        {
            rechargable.ChargeTimeSnapshot = battleTime;
            rechargable.IsOnCooldown = true;
            return rechargable.GetValue();
        }
        public static void UpdateCooldownState(this Rechargeable rechargable, double battleTime)
        {
            var reloadTime = rechargable.GetReloadTime();
            if (rechargable.ChargeTimeSnapshot == 0)
            {
                rechargable.IsOnCooldown = battleTime % reloadTime != 0;
            }
            else
            {
                rechargable.IsOnCooldown = battleTime - rechargable.ChargeTimeSnapshot != reloadTime;
            }
        }
        public static double GetDefenceValue(this ShipPart defensiveIteam)
            => defensiveIteam.Bonuses
            .Where(x => x.Type == BonusType.Shield || x.Type == BonusType.Armor)
            .Select(x => x.Value)
            .Sum();
        public static void SetDefenceValue(this ShipPart defensiveIteam, double value)
        {
            foreach (var bonus in defensiveIteam.Bonuses)
            {
                if (bonus.Type == BonusType.Shield
                    || bonus.Type == BonusType.Armor)
                {
                    bonus.Value += value;
                }
            }
        }
        public static void LogToFile(this StringBuilder stringBuilder, TextWriter textWriter, bool isPrintedToConsole = false)
        {
            textWriter.WriteLine(stringBuilder);
            textWriter.Flush();

            if (isPrintedToConsole)
            {
                Console.WriteLine(stringBuilder);
            }
        }
    }
}