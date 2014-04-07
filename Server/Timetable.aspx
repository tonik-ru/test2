<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Timetable.aspx.cs" Inherits="Timetable" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
	<dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="False" 
	Text="Add new">
	<ClientSideEvents Click="function(s, e) {
	gvTT.AddNewRow();
}" />
</dx:ASPxButton>
	<br />
<dx:ASPxGridView ID="gvTT" runat="server" AutoGenerateColumns="False" 
	DataSourceID="edsTT" KeyFieldName="ID" 
	OnRowInserting="gvTT_RowInserting" ClientInstanceName="gvTT" 
		OnCellEditorInitialize="gvTT_CellEditorInitialize" 
		OnCustomColumnDisplayText="gvTT_CustomColumnDisplayText" 
		OnParseValue="gvTT_ParseValue">
	<Columns>
		<dx:GridViewCommandColumn VisibleIndex="0">
			<EditButton Visible="True">
			</EditButton>
			<DeleteButton Visible="True">
			</DeleteButton>
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTimeEditColumn FieldName="StartTime" VisibleIndex="3">
		</dx:GridViewDataTimeEditColumn>
		<dx:GridViewDataTimeEditColumn FieldName="EndTime" VisibleIndex="4">
		</dx:GridViewDataTimeEditColumn>
		<dx:GridViewDataTextColumn FieldName="MaxThreads" VisibleIndex="5">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Delay" VisibleIndex="6">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTimeEditColumn Caption="Work limit" FieldName="MaxSeconds" 
			VisibleIndex="7">
			<PropertiesTimeEdit DisplayFormatString="">
			</PropertiesTimeEdit>
		</dx:GridViewDataTimeEditColumn>
	</Columns>
	<SettingsEditing Mode="PopupEditForm" />
	<SettingsPopup>
		<EditForm MinWidth="500px" />
	</SettingsPopup>
</dx:ASPxGridView>
<asp:EntityDataSource ID="edsTT" runat="server" 
	ConnectionString="name=EDFServerEntities" 
	DefaultContainerName="EDFServerEntities" EnableDelete="True" 
	EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
	EntitySetName="Timetables">
</asp:EntityDataSource>
</asp:Content>

