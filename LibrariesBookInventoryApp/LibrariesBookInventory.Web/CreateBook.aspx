<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"  CodeBehind="~/CreateBook.aspx.cs" Inherits="LibrariesBookInventory.CreateBook"  Async="true"%>
<%@ Register TagPrefix="control" TagName="BookForm" Src="~/BookForm.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="col-md-12">
            <div>
                <h2>Create book</h2>
                <control:BookForm ID="BookForm" runat="server" />

                <asp:Button ID="btnCteate" runat="server" Text="Create" OnClick="BtnCreate_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Calcel" OnClick="BtnCancel_Click" />
            </div>
        </section>
    </main>
</asp:Content>