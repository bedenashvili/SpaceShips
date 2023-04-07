using System.Collections.Generic;
using System.Text;

namespace SpaceBattle
{
    public class Ship
    {
        public string Name { get; set; }
        public List<Armor> ArmorDeck { get; set; }
        public List<Shield> ShieldDeck { get; set; }
        public List<Ordnance> GunDeck { get; set; }
        public List<Module> ModuleDeck { get; set; }
        public Ship(string name)
        {
            Name = name;
            ArmorDeck = new()
            {
                new Armor(name: "Base Armor", type: BonusType.Armor, armor: 100)
            };
            ShieldDeck = new()
            {
                new Shield("Base Shield", BonusType.Shield, 120, BonusType.ReloadTime,
                1, BonusType.ShieldRepaired, 1)
            };
            GunDeck = new()
            {
                new Ordnance(name: "Base Gun A", damageType: BonusType.GunDamage, damage: 5, reloadType: BonusType.ReloadTime, reloadTime: 3),
                new Ordnance(name: "Base Gun B", damageType: BonusType.GunDamage, damage: 4, reloadType: BonusType.ReloadTime, reloadTime: 2)
            };
            ModuleDeck = new()
            {
                new Module("Base Shield", new Bonus (type: BonusType.Shield, value: 50, isMultiplicative: false)),
                new Module("Base Armor", new Bonus (type: BonusType.Armor, value: 50, isMultiplicative: false))
            };
        }
        public Ship(string name, double armor, double shield, double shieldRechargeTime, double shieldRechargeAmount,
            int gunPorts, int modulePorts, List<Ordnance> gunDataBase, List<Module> moduleDataBase)
        {
            Name = name;
            ArmorDeck = new()
            {
                new Armor(name: "Base Armor", type: BonusType.Armor, armor: armor)
            };
            ShieldDeck = new()
            {
                new Shield("Base Shield", BonusType.Shield, shield, BonusType.ReloadTime,
                shieldRechargeTime, BonusType.ShieldRepaired, shieldRechargeAmount)
            };
            GunDeck = Shipyard<Ordnance>.DeckConstructor(gunPorts, gunDataBase, name);
            ModuleDeck = Shipyard<Module>.DeckConstructor(modulePorts, moduleDataBase, name);
        }
        public Ship(string name, int armorPorts, int shieldPorts, int gunPorts, int modulePorts,
            List<Armor> armorDatabase, List<Shield> shieldDatabase, List<Ordnance> gunDataBase, List<Module> moduleDataBase)
        {
            Name = name;
            ArmorDeck = Shipyard<Armor>.DeckConstructor(armorPorts, armorDatabase, name);
            ShieldDeck = Shipyard<Shield>.DeckConstructor(shieldPorts, shieldDatabase, name);
            GunDeck = Shipyard<Ordnance>.DeckConstructor(gunPorts, gunDataBase, name);
            ModuleDeck = Shipyard<Module>.DeckConstructor(modulePorts, moduleDataBase, name);
        }
        public void ApplyBonuses(bool isPrinted = false)
        {
            List<(BaseValue bonus, string itemInDeck)> shipBonuses = GetShipBonuses();
            List<Bonus> moduleBonuses = GetModuleBonuses();
            StringBuilder log = new();
            log.Append($"Applying bonuses to {Name}\n\n");

            for (int i = 0; i < moduleBonuses.Count; i++)
            {
                Bonus bonus = moduleBonuses[i];
                for (int j = 0; j < shipBonuses.Count; j++)
                {
                    BaseValue baseValue = shipBonuses[j].bonus;
                    string itemName = shipBonuses[j].itemInDeck;
                    if (bonus.Type == baseValue.Type)
                    {
                        log.Append($"{itemName,-10}\tType: {baseValue.Type,-10}\tCurrent Value: {baseValue.Value,-5:f2}");
                        if (bonus.IsMultiplicative)
                        {
                            log.Append($"\tBonus: {bonus.Value,-5:p2}");
                            baseValue.Value *= (1 + bonus.Value);
                        }
                        else
                        {
                            log.Append($"\tBonus: {bonus.Value,-5:f2}");
                            baseValue.Value += bonus.Value;
                        }
                        log.Append($"\tResult: {baseValue.Value:f2}\n");
                    }
                }
            }
            ArmorDeck.ForEach(x => x.Capacity = x.GetDefenceValue());
            ShieldDeck.ForEach(x => x.Capacity = x.GetDefenceValue());
            if (isPrinted)
            {
                System.Console.WriteLine(log);
                log.Clear();
            }
        }
        private List<Bonus> GetModuleBonuses()
        {
            List<Bonus> moduleBonuses = new();
            foreach (var module in ModuleDeck)
            {
                foreach (var bonus in module.Bonuses)
                {
                    moduleBonuses.Add(bonus);
                }
            }
            return moduleBonuses;
        }
        private List<(BaseValue, string)> GetShipBonuses()
        {
            List<(BaseValue, string)> shipBonuses = new();

            foreach (var module in ArmorDeck)
            {
                GetDeckBonuses(shipBonuses, module);
            }
            foreach (var module in ShieldDeck)
            {
                GetDeckBonuses(shipBonuses, module);
            }
            foreach (var module in GunDeck)
            {
                GetDeckBonuses(shipBonuses, module);
            }
            return shipBonuses;

            static void GetDeckBonuses(List<(BaseValue, string)> shipBonuses, ShipPart module)
            {
                foreach (var bonus in module.Bonuses)
                {
                    shipBonuses.Add((bonus, module.Name));
                }
            }
        }
    }
}