<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Modules.aspx.cs" Inherits="Modules" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
	<dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="False" 
	Text="Add new module">
	<ClientSideEvents Click="function(s, e) {
	gvModules.AddNewRow();
}" />

</dx:ASPxButton>
	<br />
	<dx:ASPxGridView ID="gvModules" runat="server" DataSourceID="edsModules" 
	AutoGenerateColumns="False" ClientInstanceName="gvModules" KeyFieldName="Id">
		<Columns>
			<dx:GridViewCommandColumn VisibleIndex="0">
				<EditButton Visible="True">
				</EditButton>
				<DeleteButton Visible="True">
				</DeleteButton>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="1">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataHyperLinkColumn FieldName="Id" VisibleIndex="3" Caption="Usage">
				<PropertiesHyperLinkEdit NavigateUrlFormatString="module/{0}/usage" 
					Text="View usage">
				</PropertiesHyperLinkEdit>
			</dx:GridViewDataHyperLinkColumn>
		</Columns>
		<SettingsBehavior ConfirmDelete="True" />
		<SettingsPager Visible="False" Mode="ShowAllRecords" PageSize="30">
		</SettingsPager>
		<SettingsEditing Mode="PopupEditForm" />
		<SettingsPopup>
			<EditForm Width="400px" />
		</SettingsPopup>
	</dx:ASPxGridView>
<asp:EntityDataSource ID="edsModules" runat="server" 
		ConnectionString="name=EDFServerEntities" 
		DefaultContainerName="EDFServerEntities" EnableDelete="True" 
		EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
		EntitySetName="EDFModules">
</asp:EntityDataSource>
</asp:Content>

