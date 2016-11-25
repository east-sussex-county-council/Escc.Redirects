<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortUrls.aspx.cs" Inherits="Escc.Redirects.Admin.ShortUrls" EnableViewState="false" %>
<%@ Import Namespace="Escc.Html" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
<Metadata:MetadataControl runat="server" 
    Title="Short URLs"
    DateCreated="2011-04-15"
    IpsvPreferredTerms="Internet" />
    <style>
     .sort * { display: inline; margin-left: .2em; margin-right: .3em; }   
    </style>
    <ClientDependency:Css runat="server" Files="ContentSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
    <link rel="stylesheet" type="text/css" href="redirects.css"/>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="fullScreenHeading">Short URLs</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="fullScreenLinks"><p><a href="default.aspx">Back to redirects</a></p></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
<div class="full-page">
    <article>
        <div class="full-screen-content text-content">
            <p>These short URLs should be used for publicity.
               Each one is preceded by &quot;eastsussex.gov.uk/&quot;.</p>
            <p>For example &quot;arts&quot; becomes &quot;eastsussex.gov.uk/arts&quot;.</p>

            <FormControls:EsccButton runat="server" Text="Add a redirect" OnClick="AddRedirect_Click" CssClass="button" id="add" />

            <div class="sort"><p>Sort by:</p>
            <ul id="sortLinks" runat="server">
            </ul></div>

            <asp:repeater runat="server" id="viewTable">
                <HeaderTemplate>
                    <table class="data">
                        <thead><tr><th scope="col">Short URL</th><th scope="col">Redirects to</th><th scope="col">Comment</th></tr></thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td title="eastsussex.gov.uk/<%# System.Web.HttpUtility.HtmlEncode(Eval("Pattern").ToString()) %>"><%# System.Web.HttpUtility.HtmlEncode(Eval("Pattern").ToString()) %></td>
                        <td><a href="<%# System.Web.HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>" title="<%# System.Web.HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>">
                                <%# System.Web.HttpUtility.HtmlEncode(new HtmlLinkFormatter().AbbreviateUrl(new Uri(Eval("Destination").ToString(), UriKind.RelativeOrAbsolute), Request.Url, 55))%>
                             </a>
                        </td>
                        <td><%# System.Web.HttpUtility.HtmlEncode(Eval("Comment").ToString())%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>        
                </FooterTemplate>
            </asp:repeater>

            <asp:repeater runat="server" id="editTable">
                <HeaderTemplate>
                    <table class="data">
                        <thead><tr><th scope="col">Short URL</th><th scope="col">Redirects to</th><th scope="col">Comment</th><th>Edit</th><th>Delete</th></tr></thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td title="eastsussex.gov.uk/<%# System.Web.HttpUtility.HtmlEncode(Eval("Pattern").ToString()) %>"><%# System.Web.HttpUtility.HtmlEncode(Eval("Pattern").ToString()) %></td>
                        <td><a href="<%# System.Web.HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>" title="<%# System.Web.HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>">
                                <%# System.Web.HttpUtility.HtmlEncode(new HtmlLinkFormatter().AbbreviateUrl(new Uri(Eval("Destination").ToString(), UriKind.RelativeOrAbsolute), Request.Url, 55))%>
                             </a>
                        </td>
                        <td><%# System.Web.HttpUtility.HtmlEncode(Eval("Comment").ToString())%></td>
                        <td><a href="edit/edit.aspx?redirect=<%# System.Web.HttpUtility.HtmlEncode(Eval("RedirectId").ToString()) %>">Edit</a></td>
                        <td><a href="edit/delete.aspx?redirect=<%# System.Web.HttpUtility.HtmlEncode(Eval("RedirectId").ToString()) %>">Delete</a></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>        
                </FooterTemplate>
            </asp:repeater>
        </div>
        </article>
        </div>
</asp:Content>