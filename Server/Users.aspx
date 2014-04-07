<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Users" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
	<dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="False" 
	Text="New user">
	<ClientSideEvents Click="function(s, e) {
	gvUsers.AddNewRow();
}" />
</dx:ASPxButton>
	<br />
	<dx:ASPxGridView ID="gvUsers" runat="server" AutoGenerateColumns="False" 
		DataSourceID="edsUsers" KeyFieldName="ID" 
	OnRowInserting="gvUsers_RowInserting" ClientInstanceName="gvUsers" 
		EnableViewState="False">
		<Columns>
			<dx:GridViewCommandColumn VisibleIndex="0" ShowInCustomizationForm="True">
				<EditButton Visible="True">
				</EditButton>
				<DeleteButton Visible="True">
				</DeleteButton>
				<ClearFilterButton Visible="True">
				</ClearFilterButton>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataHyperLinkColumn FieldName="ID" ReadOnly="True" VisibleIndex="1" 
				ShowInCustomizationForm="True">
				<PropertiesHyperLinkEdit NavigateUrlFormatString="User/{0}" TextField="ID">
				</PropertiesHyperLinkEdit>
				<EditFormSettings Visible="False" />
			</dx:GridViewDataHyperLinkColumn>
			<dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2" 
				ShowInCustomizationForm="True">
				<PropertiesTextEdit DisplayFormatString="{0}">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" EnableViewState="false"
						NavigateUrl='<%# "User/"+Eval("ID") %>' Text='<%# Eval("Name") %>' />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="SCEAPIKey" VisibleIndex="3" 
				ShowInCustomizationForm="True">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Timetable" FieldName="TimetableID" 
				VisibleIndex="4" ShowInCustomizationForm="True">
				<PropertiesComboBox DataSourceID="edsTT" TextField="Name" ValueField="ID" 
					ValueType="System.Int32">
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataCheckColumn FieldName="IsActive" VisibleIndex="5" 
				ShowInCustomizationForm="True">
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataHyperLinkColumn Caption="Usage" FieldName="ID" VisibleIndex="6">
				<PropertiesHyperLinkEdit NavigateUrlFormatString="user/{0}/usage" 
					Text="View usage">
				</PropertiesHyperLinkEdit>
				<EditFormSettings Visible="False" />
			</dx:GridViewDataHyperLinkColumn>
		</Columns>
		<SettingsBehavior ConfirmDelete="True" />
		<SettingsPager Mode="ShowAllRecords">
		</SettingsPager>
		<SettingsEditing Mode="PopupEditForm" />
		<Settings ShowFilterRow="True" />
		<SettingsPopup>
			<EditForm HorizontalAlign="WindowCenter" MinWidth="500px" 
				VerticalAlign="WindowCenter" />
		</SettingsPopup>
	</dx:ASPxGridView>
	<asp:EntityDataSource ID="edsTT" runat="server" 
	ConnectionString="name=EDFServerEntities" 
	DefaultContainerName="EDFServerEntities" 
	EnableFlattening="False" 
	EntitySetName="Timetables" EnableViewState="False" 
		StoreOriginalValuesInViewState="False">
</asp:EntityDataSource>
	<asp:EntityDataSource ID="edsUsers" runat="server" 
		ConnectionString="name=EDFServerEntities" 
		DefaultContainerName="EDFServerEntities" EnableDelete="True" 
		EnableFlattening="False" EnableInsert="True" EnableUpdate="True" 
		EntitySetName="SCEUsers" EnableViewState="False" 
		StoreOriginalValuesInViewState="False">
	</asp:EntityDataSource>
</asp:Content>

