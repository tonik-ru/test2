using DevExpress.Data.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Usage : System.Web.UI.Page
{
	int userID;
	int moduleID;

	protected void Page_Load(object sender, EventArgs e)
    {
		LinqServerModeSource  ls = new LinqServerModeSource();
		ls.KeyExpression = "Id";

		var tmpID = Page.RouteData.Values["uid"];
		int.TryParse((string)tmpID, out userID);

		var tmpmID = Page.RouteData.Values["mid"];
		int.TryParse((string)tmpmID, out moduleID);

		ls.QueryableSource = ModulesManager.GetModuleUsage(moduleID, userID);
		gvUsage.DataSourceID = null;
		gvUsage.DataSource = ls;

		edsUsage.Include = "EDFModules, SCEUsers";
    }
	
	protected void edsUsage_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
	{
	
	}
	protected void edsUsage_Selecting1(object sender, EntityDataSourceSelectingEventArgs e)
	{
		return;
		if (userID != 0)
			e.DataSource.WhereParameters.Add("UserId", System.Data.DbType.Int32, userID.ToString());
		if (moduleID != 0)
			e.DataSource.WhereParameters.Add("ModuleId", System.Data.DbType.Int32, moduleID.ToString());
	}

	protected void gvUsage_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
	{
		if (e.Column.FieldName == "WorkTime")
			e.DisplayText = TimeSpan.FromSeconds((int)e.Value).ToString("hh\\:mm\\:ss");
	}

}