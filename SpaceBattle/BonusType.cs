
namespace SpaceBattle
{
    public enum BonusType
    {
        Armor,
        Shield,
        ShieldRepaired,
        ReloadTime,
        GunDamage
    }
    public class BaseValue
    {
        public string Name { get; init; } = string.Empty;
        public BonusType Type { get; init; }
        public double Value { get; set; }
        public BaseValue() { }
        public BaseValue(BonusType type, double value)
        {
            Name = type.ToString();
            Type = type;
            Value = value;
        }
        public override string ToString()
            => $"Type: {Type}; Value: {Value}]";
        public override bool Equals(object? obj)
            => obj?.ToString() == ToString();
        public override int GetHashCode()
            => Type.GetHashCode() + Value.GetHashCode();
    }
    public class Bonus : BaseValue
    {
        public bool IsMultiplicative { get; set; }
        public Bonus() { }
        public Bonus(BonusType type, double value, bool isMultiplicative)
            : base(type, value)
        {
            IsMultiplicative = isMultiplicative;
        }
        public override string ToString()
            => $"Type: {Type}; Value: {Value}; Multiplicative: {IsMultiplicative}]";
        public override bool Equals(object? obj)
            => obj?.ToString() == ToString();
        public override int GetHashCode()
            => Type.GetHashCode() + Value.GetHashCode() + IsMultiplicative.GetHashCode();
    }
}