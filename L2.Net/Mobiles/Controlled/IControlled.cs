namespace L2.Net.Mobiles
{
    /// <summary>
    /// Represents method that will handle '<see cref="IMaster"/> under attack' event.
    /// </summary>
    /// <param name="attackers"></param>
    public delegate void MasterAttackedEventHandler( params Mobile[] attackers );

    /// <summary>
    /// Represents method that will handle <see cref="IMaster"/> clall mobile to come.
    /// </summary>
    public delegate void MasterCallEventHandler();

    /// <summary>
    /// Controlled creature interface.
    /// </summary>
    public interface IControlled : IMasterOrControlled
    {
        /// <summary>
        /// Occurs when <see cref="IMaster"/> is under attack.
        /// </summary>
        event MasterAttackedEventHandler OnMasterAttacked;

        /// <summary>
        /// Occurs when <see cref="IMaster"/> calls <see cref="IControlled"/> mobile.
        /// </summary>
        event MasterCallEventHandler OnMasterCalls;

        /// <summary>
        /// Gets <see cref="Actor"/> that currently controls creature.
        /// </summary>
        Actor Master { get; }
    }
}
