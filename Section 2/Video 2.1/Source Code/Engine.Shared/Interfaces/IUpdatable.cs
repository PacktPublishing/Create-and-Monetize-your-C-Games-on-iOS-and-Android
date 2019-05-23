using System;

namespace Engine.Shared.Interfaces
{
    /// <summary> This interface will be used for objects that can be updated regularly </summary>
    public interface IUpdatable
    {
        /// <summary> Called to update the object </summary>
        /// <param name="timeTilUpdate"></param>
        void Update(TimeSpan timeTilUpdate);
        /// <summary> Whether or not the object can be updated </summary>
        /// <returns></returns>
        Boolean CanUpdate();
    }
}
