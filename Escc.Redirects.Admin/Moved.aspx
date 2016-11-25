<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Moved.aspx.cs" Inherits="Escc.Redirects.Admin.Moved" EnableViewState="false" %>
<%@ Import Namespace="Escc.Html" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
<Metadata:MetadataControl runat="server" 
    Title="Moved pages"
    DateCreated="2011-04-15"
    IpsvPreferredTerms="Internet" />
    <ClientDependency:Css runat="server" Files="ContentSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
    <link rel="stylesheet" type="text/css" href="redirects.css"/>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="fullScreenHeading">Moved pages</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="fullScreenLinks"><p><a href="default.aspx">Back to redirects</a></p></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
<div class="full-page">
    <article>
        <div class="full-screen-content text-content">
            <p>These pages and sections on the eastsussex.gov.uk have moved, but visitors are redirected to the new location.</p>
            <p>If the page requested by the visitor <em>starts with</em> the previous location shown below, that's enough. It doesn't have to be an exact match.</p>

            <FormControls:EsccButton runat="server" Text="Add a redirect" OnClick="AddRedirect_Click" CssClass="button" id="add" />

           <div class="sort"><p>Sort by:</p>
            <ul id="sortLinks" runat="server">
            </ul></div>

            <asp:repeater runat="server" id="editTable">
                <HeaderTemplate>
                    <table class="data">
                        <thead><tr><th scope="col">Moved from</th><th scope="col">Moved to</th><th scope="col">Comment</th><th>Edit</th><th>Delete</th></tr></thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td title="<%# System.Web.HttpUtility.HtmlEncode("/" + Eval("Pattern").ToString()) %>"><%# HttpUtility.HtmlEncode(new HtmlLinkFormatter().AbbreviateUrl(new Uri("/" + Eval("Pattern"), UriKind.RelativeOrAbsolute), Request.Url, 37)) %></td>
                        <td><a href="<%# System.Web.HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>" title="<%# HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>">
                                <%# System.Web.HttpUtility.HtmlEncode(new HtmlLinkFormatter().AbbreviateUrl(new Uri(Eval("Destination").ToString(), UriKind.RelativeOrAbsolute), Request.Url, 37))%>
                             </a>
                         </td>
                        <td><%# System.Web.HttpUtility.HtmlEncode(Eval("Comment").ToString())%></td>
                        <td><a href="edit/edit.aspx?redirect=<%# HttpUtility.HtmlEncode(Eval("RedirectId").ToString()) %>">Edit</a></td>
                        <td><a href="edit/delete.aspx?redirect=<%# HttpUtility.HtmlEncode(Eval("RedirectId").ToString()) %>">Delete</a></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>        
                </FooterTemplate>
            </asp:repeater>
            
             <asp:repeater runat="server" id="viewTable">
                <HeaderTemplate>
                    <table class="data">
                        <thead><tr><th scope="col">Moved from</th><th scope="col">Moved to</th><th scope="col">Comment</th></tr></thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td title="<%# HttpUtility.HtmlEncode("/" + Eval("Pattern").ToString()) %>"><%# HttpUtility.HtmlEncode(new HtmlLinkFormatter().AbbreviateUrl(new Uri("/" + Eval("Pattern"), UriKind.RelativeOrAbsolute), Request.Url, 37)) %></td>
                        <td><a href="<%# HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>" title="<%# HttpUtility.HtmlEncode(Eval("Destination").ToString()) %>">
                                <%# HttpUtility.HtmlEncode(new HtmlLinkFormatter().AbbreviateUrl(new Uri(Eval("Destination").ToString(), UriKind.RelativeOrAbsolute), Request.Url, 37))%>
                             </a>
                         </td>
                        <td><%# HttpUtility.HtmlEncode(Eval("Comment").ToString())%></td>
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

<asp:Content runat="server" ContentPlaceHolderID="atoz" />
<asp:Content runat="server" ContentPlaceHolderID="feedback" />
<asp:Content runat="server" ContentPlaceHolderID="supporting" />