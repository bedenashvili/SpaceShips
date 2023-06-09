﻿using System.Collections.Generic;

namespace SpaceBattle
{
    internal class Battle
    {
        public static List<ShipActionProcessor> UpdateShipStates(List<Ship> shipsInBattle, int time)
        {
            List<ShipActionProcessor> ListofShipStates = new();
            ShipAimingProcessor fromShip;
            ShipActionProcessor shipState;
            for (int j = 0; j < shipsInBattle.Count; j++)
            {
                Ship ship = shipsInBattle[j];
                fromShip = new(ship, shipsInBattle);
                shipState = new(fromShip, ship, time);
                ListofShipStates.Add(shipState);
            }
            List<double> damageByShip = new();
            for (int k = 0; k < shipsInBattle.Count; k++)
            {
                double damage = default;
                Ship ship = shipsInBattle[k];
                for (int l = 0; l < shipsInBattle.Count; l++)
                {
                    Ship enemyShip = shipsInBattle[l];
                    if (ship == enemyShip)
                    {
                        continue;
                    }
                    damage += GetCumulativeDamage(ListofShipStates[l].GunData, ship);
                }
                damageByShip.Add(damage);
            }
            for (int m = 0; m < shipsInBattle.Count; m++)
            {
                Ship ship = shipsInBattle[m];
                UpdateShipDefences(ship, damageByShip[m]);
            }
            return ListofShipStates;
        }
        private static void UpdateShipDefences(Ship ship, double damage)
        {
            double remaindedDamage = damage;
            foreach (var shield in ship.ShieldDeck)
            {
                if (shield.GetDefenceValue() == 0)
                {
                    continue;
                }
                else
                {
                    if (shield.GetDefenceValue() >= remaindedDamage)
                    {
                        shield.SetDefenceValue(-remaindedDamage);
                        remaindedDamage -= remaindedDamage;
                        break;
                    }
                    else if (shield.GetDefenceValue() < remaindedDamage)
                    {
                        remaindedDamage -= shield.GetDefenceValue();
                        // maybe just shield.SetDefenceValue(0) ? but (-(shield.GetDefenceValue())) is clearer, imo
                        shield.SetDefenceValue(-(shield.GetDefenceValue()));
                    }
                }
            }
            foreach (var armor in ship.ArmorDeck)
            {
                if (armor.GetDefenceValue() == 0)
                {
                    continue;
                }
                else
                {
                    if (armor.GetDefenceValue() >= remaindedDamage)
                    {
                        armor.SetDefenceValue(-remaindedDamage);
                        break;
                    }
                    else if (armor.GetDefenceValue() < remaindedDamage)
                    {
                        armor.SetDefenceValue(-(armor.GetDefenceValue()));
                        break;
                    }
                }
            }
        }
        private static double GetCumulativeDamage(List<GunData> gunsUpdateForEnemy, Ship enemy)
        {
            double cumulativeDamage = 0f;
            foreach (var (gunDamage, gunName, targetName) in gunsUpdateForEnemy)
            {
                if (targetName == enemy.Name)
                {
                    cumulativeDamage += gunDamage;
                }
            }
            return cumulativeDamage;
        }
    }
}