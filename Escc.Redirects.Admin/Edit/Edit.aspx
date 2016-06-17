<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Escc.Redirects.Admin.Edit.EditPage" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl runat="server" Title="Add a redirect" IsInSearch="false" DateCreated="2012-02-09" IpsvPreferredTerms="Internet" />
    <ClientDependency:Css runat="server" Files="FormsSmall;ContentSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="FormsMedium;ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="FormsLarge;ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
    <link rel="stylesheet" type="text/css" href="../redirects.css"/>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="fullScreenHeading"><asp:literal runat="server" id="h1">Add a redirect</asp:literal></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="fullScreenLinks"><p><a href="../default.aspx">Back to redirects</a></p></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="full-screen-content text-content">
            <div class="form service-form">
                <FormControls:EsccValidationSummary runat="server" />
                <input type="hidden" id="redirectId" runat="server" />

                <div class="formBox">
                    <div class="formPart">
                        <asp:Label runat="server" AssociatedControlID="type">Type</asp:Label>
                        <asp:DropDownList runat="server" ID="type">
                            <asp:ListItem Value="1" Text="Short URL" />
                            <asp:ListItem Value="2" Text="Page moved" />
                        </asp:DropDownList>
                    </div>
                    <FormControls:EsccRequiredFieldValidator runat="server" ControlToValidate="type" ErrorMessage="Please select the type of redirect" />

                    <div class="formPart">
                        <asp:label runat="server" AssociatedControlID="pattern">Redirect from (for example, for <b>eastsussex.gov.uk/alerts</b> type "<kbd>alerts</kbd>")</asp:label>
                        <asp:TextBox runat="server" ID="pattern" MaxLength="200" />
                    </div>
                    <FormControls:EsccRequiredFieldValidator runat="server" ControlToValidate="pattern" ErrorMessage="Please enter the URL to redirect from" />
                    <FormControls:LengthValidator runat="server" ControlToValidate="pattern" MaximumLength="200" ErrorMessage="The URL to redirect from must be 200 characters or fewer" />


                    <div class="formPart">
                        <asp:Label runat="server" AssociatedControlID="destination">Redirect to</asp:Label>
                        <asp:TextBox runat="server" ID="destination" MaxLength="200" />
                    </div>
                    <FormControls:EsccRequiredFieldValidator runat="server" ControlToValidate="destination" ErrorMessage="Please enter the URL to redirect to" />
                    <FormControls:LengthValidator runat="server" ControlToValidate="destination" MaximumLength="200" ErrorMessage="The URL to redirect to must be 200 characters or fewer" />
                    <FormControls:UrlValidator runat="server" ControlToValidate="destination" ErrorMessage="The URL to redirect to is not valid" />

                    <div class="formPart">
                        <asp:Label runat="server" AssociatedControlID="comment">Notes (for example, where is a short URL used and how long is it needed? Up to 200 characters.)</asp:Label>
                        <asp:TextBox runat="server" ID="comment" TextMode="MultiLine" MaxLength="200" />
                    </div>
                    <FormControls:LengthValidator runat="server" ControlToValidate="comment" MaximumLength="200" ErrorMessage="Your notes must be 200 characters or fewer" />
                </div>

                <FormControls:EsccButton runat="server" Text="Submit" OnClick="Submit_Click" />
            </div>
        </div>
    </div>
</asp:Content>