#region

using System;
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Stormriders.Builders.Interfaces;

[PublicAPI]
public interface ILeaderBuilder : ICardBuilder
{
    bool LeaderExclusive { get; }
    LeaderModifier LeaderModifiers { get; }
    bool InPool { get; }

    public struct LeaderModifier()
    {
        public Vector2Int healthRange = Vector2Int.zero;
        public Vector2Int damageRange = Vector2Int.zero;
        public Vector2Int counterRange = Vector2Int.zero;
        public Action<CardData> subscribe = delegate { };
    }
}