using System.Collections.Generic;
using System.Linq;

namespace SpaceBattle
{
    public abstract class Shipyard<Factory>
    {
        public static List<Factory> DeckConstructor(int totalSlots, List<Factory> dataBase, string name)
        {
            List<Factory> itemsFromDatabase = dataBase.GetItemsFromDatabase();

            List<Factory> deck = new();

            for (int i = 0; i < totalSlots; i++)
            {
                itemsFromDatabase.PrintCollection(totalSlots, i, name);
                int chosenElementIndex = itemsFromDatabase.GetUserChoice();
                if (chosenElementIndex >= 0)
                {
                    Factory chosenElement = itemsFromDatabase[chosenElementIndex];
                    deck.Add(chosenElement);
                    itemsFromDatabase.Remove(chosenElement);
                }
            }
            return deck;
        }
    }
    public abstract class Enhancement : Shipyard<Bonus>
    {
        public string Name { get; set; } = string.Empty;
        public int TotalSlots { get; set; }
        public List<Bonus> Bonuses { get; set; } = new();
        public Enhancement() { }
        public Enhancement(string name, int totalSlots, List<Bonus> bonusesDatabase)
        {
            Name = name;
            Bonuses = DeckConstructor(totalSlots, bonusesDatabase, name);
        }
        public Enhancement(string name, Bonus bonus)
        {
            Name = name;
            Bonuses.Add(bonus);
        }
    }
    public abstract class ShipPart : Shipyard<BaseValue>
    {
        public string Name { get; set; } = string.Empty;
        public int TotalSlots { get; set; }
        public List<BaseValue> Bonuses { get; set; } = new();
        public ShipPart() { }
        public ShipPart(string name, int totalSlots, List<BaseValue> bonusesDatabase)
        {
            Name = name;
            Bonuses = DeckConstructor(totalSlots, bonusesDatabase, name);
        }
        public ShipPart(string name, BonusType type, double value)
        {
            Name = name;
            Bonuses.Add(new()
            {
                Name = type.ToString(),
                Value = value,
                Type = type
            });
        }
    }
    public abstract class Rechargeable : ShipPart
    {
        public bool IsOnCooldown { get; set; }
        public double ChargeTimeSnapshot { get; set; }
        public Rechargeable() { }
        public Rechargeable(string name, int totalSlots, List<BaseValue> bonusesDatabase)
            : base(name, totalSlots, bonusesDatabase) { }
        public Rechargeable(string name, BonusType type1, double bonus1, BonusType type2, double bonus2)
            : base(name, type1, bonus1)
        {
            Bonuses.Add(new()
            {
                Name = type2.ToString(),
                Type = type2,
                Value = bonus2
            });
        }
    }
    public abstract class Repairable : Rechargeable
    {
        public Repairable() { }
        public Repairable(string name, int totalSlots, List<BaseValue> bonusesDatabase)
            : base(name, totalSlots, bonusesDatabase) { }
        public Repairable(string name, BonusType capacityType, double capacity, BonusType reloadType, double reloadTime, BonusType reloadAmountType, double reloadAmount)
            : base(name, capacityType, capacity, reloadType, reloadTime)
        {
            Bonuses.Add(new()
            {
                Name = reloadAmountType.ToString(),
                Type = reloadAmountType,
                Value = reloadAmount
            });
        }
    }
    public class Module : Enhancement
    {
        public Module() { }
        public Module(string name, int totalSlots, List<Bonus> bonusesDatabase)
            : base(name, totalSlots, bonusesDatabase) { }
        public Module(string name, Bonus bonus)
            : base(name, bonus) { }
        public override string ToString()
            => $"[Name: {Name}; Number of bonuses: {Bonuses.Count}; Bonuses: {Bonuses.PrintBonuses()}]";
    }
    public class Armor : ShipPart
    {
        public double Capacity { get; set; }
        public Armor() { }
        public Armor(string name, int totalSlots, List<BaseValue> bonusesDatabase)
            : base(name, totalSlots, bonusesDatabase)
        {
            Capacity = Bonuses.Where(x => x.Type == BonusType.Armor).Select(x => x.Value).Sum();
        }
        public Armor(string name, BonusType type, double armor)
            : base(name, type, armor)
        {
            Capacity = armor;
        }
        public override string ToString()
            => $"[Name: {Name}; Number of bonuses: {Bonuses.Count}; Bonuses: {Bonuses.PrintBonuses()}]";
    }
    public class Ordnance : Rechargeable
    {
        public Ordnance() { }
        public Ordnance(string name, int totalSlots, List<BaseValue> bonusesDatabase)
            : base(name, totalSlots, bonusesDatabase) { }
        public Ordnance(string name, BonusType damageType, double damage, BonusType reloadType, double reloadTime)
            : base(name, damageType, damage, reloadType, reloadTime) { }
        public override string ToString()
            => $"[Name: {Name}; Number of bonuses: {Bonuses.Count}; Bonuses: {Bonuses.PrintBonuses()}]";
    }
    public class Shield : Repairable
    {
        public double Capacity { get; set; }
        public Shield() { }
        public Shield(string name, int totalSlots, List<BaseValue> bonusesDatabase)
            : base(name, totalSlots, bonusesDatabase)
        {
            Capacity = Bonuses.Where(x => x.Type == BonusType.Shield).Select(x => x.Value).Sum();
        }
        public Shield(string name, BonusType capacityType, double capacity, BonusType reloadType, double reloadTime, BonusType reloadAmountType, double reloadAmount)
            : base(name, capacityType, capacity, reloadType, reloadTime, reloadAmountType, reloadAmount)
        {
            Capacity = capacity;
        }
        public override string ToString()
            => $"[Name: {Name}; Number of bonuses: {Bonuses.Count}; Bonuses: {Bonuses.PrintBonuses()}]";
    }
}