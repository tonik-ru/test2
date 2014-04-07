<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="UserApps" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
	<asp:Panel ID="pValid" runat="server" >
		Select availble applications for user
		<dx:ASPxLabel ID="lblUser" runat="server" CssClass="label">
		</dx:ASPxLabel>
		<dx:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click">
		</dx:ASPxButton>
		<dx:ASPxListBox ID="lbUserApps" runat="server" SelectionMode="CheckColumn" 
			EnableTheming="True" Rows="500" cssclass="autoHeight">

			<InvalidStyle CssClass="ss">
			</InvalidStyle>

		</dx:ASPxListBox>
	</asp:Panel>

	<asp:Panel ID="pWrong" runat="server" EnableViewState="False" CssClass="message-error">
		Unknown user
	</asp:Panel>
	</asp:Content>

