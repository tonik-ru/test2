using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class UndoManager
	{
		protected int maxUndoItems;
		public List<UndoData> UndoDataList { get; set; }

		public ISimpleScraper Scraper { get; protected set; }

		public UndoManager(ISimpleScraper scrap)
		{
			Scraper = scrap;
			UndoDataList = new List<UndoData>();
			maxUndoItems = 100000;
		}

		public UndoData CreateUndo()
		{
			var data = new UndoData();
			var redoes = UndoDataList.Where(x => x.UndoDone).ToList();
			redoes.ForEach(x => UndoDataList.Remove(x));
			
			if (UndoDataList.Count > 0)
				data.Order = UndoDataList.Max(x => x.Order) + 1;
			UndoDataList.Add(data);
			if (UndoDataList.Count > maxUndoItems)
				UndoDataList.RemoveAt(0);
			return data;
		}

		public bool HasUndoSteps()
		{
			return UndoDataList.Where(x => !x.UndoDone).Any();
		}

		public bool HasRedoSteps()
		{
			return UndoDataList.Where(x => x.UndoDone).Any();
		}

		public void UndoLastStep()
		{
			var lastUndo = UndoDataList.Where(x => !x.UndoDone).OrderBy(x => x.Order).LastOrDefault();
			if (lastUndo == null)
				return;
			lastUndo.Undo();
		}

		public void RedoLastStep()
		{
			var lastUndo = UndoDataList.Where(x => x.UndoDone).OrderBy(x => x.Order).FirstOrDefault();
			if (lastUndo == null)
				return;
			lastUndo.Redo();
		}

	}
}