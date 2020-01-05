using System;

namespace DavidBerry.Framework.Domain
{
    /// <summary>
    /// Enum with values to track object state
    /// </summary>
    public enum ObjectState
    {
        NEW,
        UNCHANGED,
        MODIFIED,
        DELETED
    }

}
