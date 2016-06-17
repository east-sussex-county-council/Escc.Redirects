<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Escc.Redirects.Admin.Edit.Delete" %>
<asp:content runat="server" COntentPlaceholderId="metadata">
    <Metadata:MetadataControl runat="server" Title="Delete a redirect" IsInSearch="false" DateCreated="2012-02-09" IpsvPreferredTerms="Internet" />
    <ClientDependency:Css runat="server" Files="ContentSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
    <link rel="stylesheet" type="text/css" href="../redirects.css"/>
</asp:content>          
            
<asp:Content runat="server" ContentPlaceHolderID="fullScreenHeading">Delete a redirect</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="fullScreenLinks"><p><a href="../default.aspx">Back to redirects</a></p></asp:Content>

<asp:content runat="server" ContentPlaceholderId="content">
    <div class="full-page">
        <div class="full-screen-content text-content">
            <input runat="server" id="redirectId" type="hidden" />
            <input runat="server" id="type" type="hidden" />
            <p>Are you sure you want to delete the redirect from:</p>
            <p>eastsussex.gov.uk/<strong runat="server" id="pattern"></strong>?</p>
            <p>People may still have bookmarks or printed leaflets, and other sites may still link to it. 
            Could you redirect it to some current content?</p>
            
            <FormControls:EsccButton runat="server" Text="Delete it" OnClick="Delete_Click" CssClass="button" />
            <FormControls:EsccButton runat="server" Text="Edit it" OnClick="Edit_Click" CssClass="button" />
        </div>
    </div>
</asp:content>