using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideItBobby.Common
{
    internal interface IUpdatable
    {
        bool IsCurrent { get; }

        void Update();
    }

    internal interface IForceUpdatable : IUpdatable
    {
        void Update(bool force);
    }

    internal interface IUpdatable<TResult> : IUpdatable
    {
        new TResult Update();
    }
    internal interface IForceUpdatable<TResult> : IUpdatable<TResult>, IForceUpdatable
    {
        new TResult Update(bool force);
    }
}