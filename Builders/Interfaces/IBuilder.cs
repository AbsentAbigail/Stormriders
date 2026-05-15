#region

using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;

#endregion

namespace Stormriders.Builders.Interfaces;

[PublicAPI]
public interface IBuilder<T, TY>
    where T : DataFile
    where TY : DataFileBuilder<T, TY>, new()
{
    public DataFileBuilder<T, TY> Builder();
}