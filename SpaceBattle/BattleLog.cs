using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceBattle
{
    public static class BattleLog
    {
        public static void PrintBattleLog(List<Ship> shipsInBattle, int time,
            List<ShipActionProcessor> UpdatesShip, TextWriter textWriter,
            bool isPrintedInConsole = false)
        {
            StringBuilder stringBuilder = new();
            if (UpdatesShip.Any(x => x.GunData.Count > 0 || x.ShieldData.Count > 0))
            {
                stringBuilder.AppendLine(PrintTime(time));
                for (int i = 0; i < shipsInBattle.Count; i++)
                {
                    Ship? ship = shipsInBattle[i];
                    GetBattleLog(ship, UpdatesShip[i], stringBuilder);
                }
                stringBuilder.LogToFile(textWriter, isPrintedInConsole);
            }
        }
        public static string PrintTime(int time)
        {
            TimeSpan time1 = TimeSpan.FromMilliseconds(time);
            return $"[{time1.Minutes} min {time1.Seconds} sec {time1.Milliseconds} ms]";
        }
        private static void GetBattleLog(Ship ship, ShipActionProcessor UpdatesShip,
        StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"[{ship.Name}]");
            if (UpdatesShip.GunData.Count != 0)
            {
                stringBuilder.Append('\t');
                stringBuilder.AppendLine(GetDamage(UpdatesShip.GunData));
            }
            if (UpdatesShip.ShieldData.Count != 0)
            {
                stringBuilder.Append('\t');
                stringBuilder.AppendLine(GetRepairing(UpdatesShip.ShieldData));
            }
            stringBuilder.Append('\t');
            stringBuilder.AppendLine(GetShield(ship));
            stringBuilder.Append('\t');
            stringBuilder.AppendLine(GetArmor(ship));
        }
        private static string GetDamage(List<GunData> gunData)
        {
            string temp = string.Empty;
            foreach (GunData gun in gunData)
            {
                temp += $"[{gun.GunName} caused {gun.Damage:f2} damage to {gun.TargetName}]";
            }
            return temp;
        }
        private static string GetRepairing(List<ShieldData> shieldData)
        {
            string temp = string.Empty;
            foreach (ShieldData shield in shieldData)
            {
                temp += $"[{shield.Name} repaired {shield.RepairedAmount:f2}]";
            }
            return temp;
        }
        private static string GetShield(Ship ship)
        {
            string temp = string.Empty;
            foreach (var shield in ship.ShieldDeck)
            {
                temp += $"[Shield: {shield.GetDefenceValue():f2}({shield.GetDefenceValue() / shield.Capacity:p2})]";
            }
            return temp;
        }
        private static string GetArmor(Ship ship)
        {
            string temp = string.Empty;
            foreach (var armor in ship.ArmorDeck)
            {
                temp += $"[Armor: {armor.GetDefenceValue():f2}({armor.GetDefenceValue() / armor.Capacity:p2})]";
            }
            return temp;
        }
    }
}