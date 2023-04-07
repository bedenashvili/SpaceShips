using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SpaceBattle
{
    public class Instance
    {
        private readonly TextWriter battleLooger;
        private readonly bool isBattleLogPrintedInConsole;
        public List<Ship> ShipsInBattle { get; set; }
        public Instance(List<Ship> shipsInBattle, TextWriter battleLooger, bool isBattleLogPrintedInConsole = false)
        {
            this.isBattleLogPrintedInConsole = isBattleLogPrintedInConsole;
            ShipsInBattle = shipsInBattle;
            this.battleLooger = battleLooger;
        }
        public void Start()
        {
            for (int i = 0; ShipsInBattle.Count > 1; i++)
            {
                List<ShipActionProcessor> updateStates = Battle.UpdateShipStates(ShipsInBattle, i);
                BattleLog.PrintBattleLog(ShipsInBattle, i, updateStates, battleLooger, isBattleLogPrintedInConsole);
                for (int n = 0; n < ShipsInBattle.Count;)
                {
                    Ship ship = ShipsInBattle[n];
                    if (CheckIfDestroyed(ship, i))
                    {
                        ShipsInBattle.Remove(ship);
                    }
                    else
                    {
                        n++;
                    }
                }
            }
            Console.WriteLine(Outcome());
        }
        private static bool CheckIfDestroyed(Ship ship, int time)
        {
            double shield = ship.ShieldDeck.Select(x => x.GetDefenceValue()).Sum();
            double armor = ship.ArmorDeck.Select(x => x.GetDefenceValue()).Sum();
            if (shield + armor == 0)
            {
                Console.WriteLine($"{BattleLog.PrintTime(time),-20} {ship.Name} exploded!");
                return shield + armor == 0;
            }
            return false;
        }
        private string Outcome()
        {
            return ShipsInBattle.Count switch
            {
                > 0 => $"{ShipsInBattle[0].Name} won!",
                _ => "Tie. No ship survived!",
            };
        }
    }
}