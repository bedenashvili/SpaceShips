using System;
using System.Collections.Generic;

namespace SpaceBattle
{
    public class ShipAimingProcessor
    {
        private readonly Random _random = new();
        public List<string> ListOfEnemies { get; set; } = new();
        public List<(Rechargeable rechargeable, string enemyName)> AimedDevices { get; set; } = new();
        public ShipAimingProcessor(Ship ship, List<Ship> ships)
        {
            ListOfEnemies = GetListOfEnemies(ship, ships);
            AimedDevices = GetDeviceList(ship, ListOfEnemies);
        }
        private static List<string> GetListOfEnemies(Ship ship, List<Ship> shipsInBattle)
        {
            List<string> targets = new();

            foreach (Ship target in shipsInBattle)
            {
                if (target.Name != ship.Name)
                {
                    targets.Add(target.Name);
                }
            }
            return targets;
        }
        private string GetTarget(List<string> listOfEnemies)
            => listOfEnemies[_random.Next(0, listOfEnemies.Count)];
        private (Rechargeable gun, string gunTarget) AddTargetToTrackingDevice(Rechargeable device, List<string> listOfEnemies)
            => (device, GetTarget(listOfEnemies));
        private List<(Rechargeable gun, string gunTarget)> GetDeviceList(Ship ship, List<string> listOfEnemies)
        {
            List<(Rechargeable gun, string gunTarget)> deviceList = new();
            foreach (var gun in ship.GunDeck)
            {
                deviceList.Add(AddTargetToTrackingDevice(gun, listOfEnemies));
            }
            return deviceList;
        }
    }
}