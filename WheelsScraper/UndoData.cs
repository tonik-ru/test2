using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class UndoData
	{
		public string Name { get; set; }
		public int Order { get; set; }
		public List<UndoItem> ChangedObjects { get; set; }
		public bool UndoDone { get; set; }

		public UndoData()
		{
			ChangedObjects = new List<UndoItem>();
		}

		public void AddItem(object obj, string propName, object originalVal, object updatedVal)
		{
			var item = new UndoItem { Object = obj, OriginalValue = originalVal, NewValue = updatedVal, PropertyName = propName };
			ChangedObjects.Add(item);
		}

		public void Undo()
		{
			foreach (var item in ChangedObjects)
			{
				Scraper.Lib.Main.PropHelper.SetPropValue(item.Object, item.PropertyName, item.OriginalValue);
			}
			UndoDone = true;
		}
		public void Redo()
		{
			foreach (var item in ChangedObjects)
			{
				Scraper.Lib.Main.PropHelper.SetPropValue(item.Object, item.PropertyName, item.NewValue);
			}
			UndoDone = false;
		}
	}

	public class UndoItem
	{
		public object Object { get; set; }
		public string PropertyName{get;set;}
		public object OriginalValue{get;set;}
		public object NewValue{get;set;}
	}
}
