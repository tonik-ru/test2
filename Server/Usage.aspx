<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Usage.aspx.cs" Inherits="Usage" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Data.Linq" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
	<dx:ASPxGridView ID="gvUsage" runat="server" AutoGenerateColumns="False" 
	DataSourceID="edsUsage" EnableViewState="False" KeyFieldName="Id" 
		OnCustomColumnDisplayText="gvUsage_CustomColumnDisplayText">
	<Columns>
		<dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="1">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="SCEUsers.Name" VisibleIndex="2" 
			Caption="User">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="EDFModules.Name" VisibleIndex="3" 
			Caption="Module">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="WorkTime" VisibleIndex="5">
			<PropertiesTextEdit>
				<MaskSettings Mask="H:mm" />
			</PropertiesTextEdit>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn FieldName="StartDate" VisibleIndex="4">
			<PropertiesDateEdit DisplayFormatString="">
			</PropertiesDateEdit>
		</dx:GridViewDataDateColumn>
	</Columns>
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterRow="True" />
</dx:ASPxGridView>
	<asp:EntityDataSource ID="edsUsage" runat="server" 
		AutoGenerateWhereClause="True" ConnectionString="name=EDFServerEntities" 
		DefaultContainerName="EDFServerEntities" EnableFlattening="False" 
		EnableViewState="False" EntitySetName="EDFUsage" 
		OnSelecting="edsUsage_Selecting1" StoreOriginalValuesInViewState="False">
	</asp:EntityDataSource>
	<asp:LinqDataSource ID="LinqDataSource1" runat="server">
	</asp:LinqDataSource>
	<dx:EntityServerModeDataSource ID="edssmUsage" runat="server" 
		ContextTypeName="EDFServerEntities" TableName="EDFUsage" />
</asp:Content>

