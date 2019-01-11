using System;
using System.Collections.Generic;
using UnityEngine;
namespace XFlag.Alter3Simulator
{
    public class TableBase<Entity, EntityID> : ScriptableObject
		where Entity : ITableEntity
		where EntityID : IConvertible
	{
		public List<Entity> list;
		public List<Entity> List { get { return list; } }

		public Entity GetEntity(EntityID id)
		{
			foreach (var entity in list)
			{
				if (entity.GetID() == id.ToInt32(null))
				{
					return entity;
				}
			}

			return default(Entity);
		}

        public int GetID(int index)
        {
            return List[index].GetID();

        }
	}
}
