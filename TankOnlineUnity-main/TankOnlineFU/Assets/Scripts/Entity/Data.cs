using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
	[Serializable]
	public class Data
	{
		public string Type;
		public Vector3 Position;
	}
	[Serializable]
	public class ListData
	{
		public List<Data> Data = new List<Data>();
	}
}
