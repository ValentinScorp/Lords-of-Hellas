using System;
using System.Collections.Generic;
internal class MonumentModel
{
    internal const int MinLevel = 1;
    internal const int MaxLevel = 5;
    internal enum GodType { Undefined, Zeus, Hermes, Athena }
    internal class PrayerSlot
    {
        private PriestModel _priest;
        internal bool IsEmpty => _priest is null;
        internal bool TryPlacePriest(PriestModel priest)
        {
            if (_priest is not null) return false;
            _priest = priest;
            return true;
        }
        internal PriestModel TakePriest()
        {
            if (_priest is null) return null;
            var priest = _priest;
            _priest = null;
            return priest;
        }
    }
    private const int _MaxPrayerSlots = 2;
    private List<PrayerSlot> _prayerSlots = new List<PrayerSlot>(_MaxPrayerSlots)
    {
        new PrayerSlot(),
        new PrayerSlot()
    };
    internal GodType God { get; }
    internal int Level { get; private set; } = MinLevel;

    internal bool IsMaxLevel => Level >= MaxLevel;
    internal bool IsMinLevel => Level <= MinLevel;

    internal MonumentModel(GodType god)
    {
        God = god;
    }
    internal int IncreaseLevel() => Level = Math.Min(Level + 1, MaxLevel);
    internal int DecreaseLevel() => Level = Math.Max(Level - 1, MinLevel);

    internal bool CanPray()
    {
        return _prayerSlots.Exists(slot => slot.IsEmpty);
    }
    internal bool TryPray(PriestModel priest)
    {
        if (priest is null) return false;

        foreach (var slot in _prayerSlots) {
            if (slot.TryPlacePriest(priest)) {
                return true;
            }
        }

        return false;
    }
}
