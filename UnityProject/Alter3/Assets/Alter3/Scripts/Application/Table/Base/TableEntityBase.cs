using System;

namespace XFlag.Alter3Simulator
{
    [Serializable]
	public class TableEntityBase<IDType> : ITableEntity where IDType : IConvertible
	{
		public IDType id;

		public int GetID()
		{
			return id.ToInt32(null);
		}
	}
}
