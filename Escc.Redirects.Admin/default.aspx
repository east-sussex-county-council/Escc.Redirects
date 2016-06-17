<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Escc.Redirects.Admin._default" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="metadata">
	<Metadata:MetadataControl id="headcontent" runat="server" 
		title="Manage redirects"
		IpsvPreferredTerms="Internet"
		DateCreated="2011-04-15"
		/>
    <ClientDependency:Css runat="server" Files="ContentSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
    <link rel="stylesheet" type="text/css" href="redirects.css"/>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="fullScreenHeading">Manage redirects</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="fullScreenLinks"><p><a href="/managewebsite/">Back to manage the website</a></p></asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="content">
<div class="full-screen-content text-content">
    <h2><a href="shorturls.aspx">Short URLs</a></h2>
    <p>Short URLs which should be used for publicity.</p>
    <h2><a href="moved.aspx">Moved pages</a></h2>
    <p>Redirects for old pages and sections which have moved. Includes some retired short URLs.</p>
</div>
</asp:Content>