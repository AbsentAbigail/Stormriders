#region

using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;

#endregion

namespace Stormriders.Builders.Interfaces;

[PublicAPI]
public interface IGameModifierBuilder : IBuilder<GameModifierData, GameModifierDataBuilder>;