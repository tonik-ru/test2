using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Users : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e)
	{
		//Trace.IsEnabled = true;
	}
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void gvUsers_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
	}
}