using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Timetable : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void gvTT_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		e.NewValues["StartTime"] = DateTime.Today.Add(((DateTime)e.NewValues["StartTime"]).TimeOfDay);
		e.NewValues["EndTime"] = DateTime.Today.Add(((DateTime)e.NewValues["EndTime"]).TimeOfDay);
	}

	protected void gvTT_CustomDataCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomDataCallbackEventArgs e)
	{

	}

	protected void gvTT_ParseValue(object sender, DevExpress.Web.Data.ASPxParseValueEventArgs e)
	{
		if (e.FieldName == "MaxSeconds")
		{
			DateTime dt;
			DateTime.TryParseExact((string)e.Value, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
			e.Value = dt.TimeOfDay.TotalSeconds;
		}
	}

	protected void gvTT_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
	{
		if (e.Column.FieldName == "MaxSeconds")
			e.DisplayText = TimeSpan.FromSeconds((int)e.Value).ToString("c");
	}

	protected void gvTT_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
	{
		if (e.Column.FieldName == "MaxSeconds")
		{
			var te = (DevExpress.Web.ASPxEditors.ASPxTimeEdit)e.Editor;
			te.Value = DateTime.Today.Add(TimeSpan.FromSeconds((int)e.Value));
		}
	}
}