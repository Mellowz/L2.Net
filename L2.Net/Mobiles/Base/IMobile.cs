namespace L2.Net.Mobiles
{
    /// <summary>
    /// Base mobiles interface.
    /// </summary>
    public interface IMobile : IUniqueIdentified, IMapUnit
    {
        /// <summary>
        /// Gets or sets mobile name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets mobile title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets value, indicating if mobile is alive.
        /// </summary>
        bool Alive { get; set; }

        /// <summary>
        /// Gets or sets value, indicating if mobile is auto-attackable.
        /// </summary>
        bool AutoAttackable { get; set; }

        /// <summary>
        /// Gets or sets value, indicating that mobile is freezed.
        /// </summary>
        bool Freezed { get; set; }

        /// <summary>
        /// Gets or sets value, indicating if mobile is visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets mobile's collision radius.
        /// </summary>
        float CollisionRadius { get; set; }

        /// <summary>
        /// Gets or sets mobile's collision height.
        /// </summary>
        float CollisionHeight { get; set; }

        /// <summary>
        /// Gets or sets mobile's level.
        /// </summary>
        sbyte Level { get; set; }

        /// <summary>
        /// Gets or sets mobile health points.
        /// </summary>
        float HP { get; set; }

        /// <summary>
        /// Gets or sets maximum value of mobile health points.
        /// </summary>
        int MaxHP { get; set; }

        /// <summary>
        /// Gets or sets mobile mana points.
        /// </summary>
        float MP { get; set; }

        /// <summary>
        /// Gets or sets maximum value of mobile mana points.
        /// </summary>
        int MaxMP { get; set; }

        /// <summary>
        /// Gets or sets mobile walk speed.
        /// </summary>
        ushort WalkSpeed { get; set; }

        /// <summary>
        /// Gets or sets mobile run speed.
        /// </summary>
        ushort RunSpeed { get; set; }

        /// <summary>
        /// Gets or sets mobile health points regeneration value.
        /// </summary>
        float HPRegen { get; set; }
        /// <summary>
        /// Gets or sets mobile mana points regeneration value.
        /// </summary>
        float MPRegen { get; set; }
        /// <summary>
        /// Gets or sets mobile health points regeneration rate value.
        /// </summary>
        float HPRegenRate { get; set; }
        /// <summary>
        /// Gets or sets mobile mana points regeneration rate value.
        /// </summary>
        float MPRegenRate { get; set; }
        /// <summary>
        /// Gets or sets mobile experience value.
        /// </summary>
        ulong Exp { get; set; }
        /// <summary>
        /// Gets or sets mobile skill points value.
        /// </summary>
        uint SP { get; set; }
        /// <summary>
        /// Gets or sets mobile STR value.
        /// </summary>
        byte STR { get; set; }
        /// <summary>
        /// Gets or sets mobile CON value.
        /// </summary>
        byte CON { get; set; }
        /// <summary>
        /// Gets or sets mobile MEN value.
        /// </summary>
        byte MEN { get; set; }
        /// <summary>
        /// Gets or sets mobile DEX value.
        /// </summary>
        byte DEX { get; set; }
        /// <summary>
        /// Gets or sets mobile WIT value.
        /// </summary>
        byte WIT { get; set; }
        /// <summary>
        /// Gets or sets mobile INT value.
        /// </summary>
        byte INT { get; set; }
        /// <summary>
        /// Gets or sets mobile physical attack strength value.
        /// </summary>
        ushort PAtk { get; set; }
        /// <summary>
        /// Gets or sets mobile magic attack strength value.
        /// </summary>
        ushort MAtk { get; set; }
        /// <summary>
        /// Gets or sets mobile physical defense value.
        /// </summary>
        ushort PDef { get; set; }
        /// <summary>
        /// Gets or sets mobile magic defense value.
        /// </summary>
        ushort MDef { get; set; }
        /// <summary>
        /// Gets or sets mobile physical attack speed.
        /// </summary>
        ushort PAtkSpd { get; set; }
        /// <summary>
        /// Gets or sets mobile magic attack speed.
        /// </summary>
        ushort MAtkSpd { get; set; }
        /// <summary>
        /// Gets or sets mobile physical attack range.
        /// </summary>
        ushort PAtkRange { get; set; }

        [System.Obsolete]
        ushort MAtkRange { get; set; }

        bool MoveTo( Point3D p );
        bool MoveTo( Mobile m );
        bool MoveTo( Mobile m, double distance );
        void DoSocialAction( byte id );
        void StopMove();
    }
}