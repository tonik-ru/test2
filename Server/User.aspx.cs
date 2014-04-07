using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserApps : System.Web.UI.Page
{
	int userID;
	SCEUsers user;

	protected void Page_Init(object sender, EventArgs e)
	{
		var tmpID = Page.RouteData.Values["uid"];
		int.TryParse((string)tmpID, out userID);
		user = UsersManager.GetUser(userID);
		
		pWrong.Visible = user == null;
		pValid.Visible = user != null;

	}

    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
			FillModules();
		if (user != null)
			lblUser.Text = user.Name;
    }



	protected void FillModules()
	{
		if (user == null)
			return;
		lbUserApps.Items.Clear();
		var modules = UsersManager.GetModules();
		var userApps = UsersManager.GetUserModules(user.ID);
		foreach (var m in modules)
		{
			var item = lbUserApps.Items.Add(m.Name, m.Id);
			item.Selected = userApps.Contains(m.Id);
		}
		//lbUserApps.DataBind();
	}

	protected void btnSave_Click(object sender, EventArgs e)
	{
		Save();
	}

	protected void Save()
	{
		var r = lbUserApps.SelectedValues.OfType<string>();
		UsersManager.SetUserApps(user.ID, r);
	}
}