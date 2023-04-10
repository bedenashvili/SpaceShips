using System.Collections.Generic;

namespace SpaceBattle
{
    public record struct GunData(double Damage, string GunName, string TargetName);
    public record struct ShieldData(double RepairedAmount, string Name);
    public class ShipActionProcessor
    {
        public List<GunData> GunData { get; set; } = new();
        public List<ShieldData> ShieldData { get; set; } = new();
        public ShipActionProcessor() { }
        public ShipActionProcessor(ShipAimingProcessor aimingProcessor, Ship ship, double battleTime)
        {
            GunData = UpdateGuns(battleTime, aimingProcessor);
            ShieldData = UpdateShields(ship, battleTime);
        }
        private static List<ShieldData> UpdateShields(Ship ship, double battleTime)
        {
            ship.ShieldDeck.ForEach(shield => { shield.UpdateCooldownState(battleTime); });

            List<ShieldData> listOfRepairedAmountPerShield = new();

            foreach (var shield in ship.ShieldDeck)
            {
                if (shield.IsOnCooldown == false
                    && shield.Capacity > shield.GetDefenceValue())
                {
                    double repairedAmount = shield.ReleaseCharge();
                    if (repairedAmount > (shield.Capacity - shield.GetDefenceValue())
                        || shield.Capacity < (repairedAmount + shield.GetDefenceValue()))
                    {
                        repairedAmount = shield.Capacity - shield.GetDefenceValue();
                        shield.SetDefenceValue(repairedAmount);
                    }
                    else
                    {
                        shield.SetDefenceValue(repairedAmount);
                    }
                    ShieldData shieldData = new(repairedAmount, shield.Name);
                    listOfRepairedAmountPerShield.Add(shieldData);
                }
            }
            return listOfRepairedAmountPerShield;
        }
        private static List<GunData> UpdateGuns(double battleTime, ShipAimingProcessor battle)
        {
            battle.AimedDevices.ForEach(gun => { gun.rechargeable.UpdateCooldownState(battleTime); });
            List<GunData> listOfGuns = new();
            foreach (var (rechargeable, enemyName) in battle.AimedDevices)
            {
                if (rechargeable.IsOnCooldown == false)
                {
                    GunData gunsData = new(rechargeable.ReleaseCharge(), rechargeable.Name, enemyName);
                    listOfGuns.Add(gunsData);
                }
            }
            return listOfGuns;
        }
    }
}