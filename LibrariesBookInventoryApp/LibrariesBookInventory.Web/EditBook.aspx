<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"  CodeBehind="~/EditBook.aspx.cs" Inherits="LibrariesBookInventory.EditBook"  Async="true"%>
<%@ Register TagPrefix="control" TagName="BookForm" Src="~/BookForm.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="col-md-12">
            <div>
                <h2>Edit Book</h2>
                <control:BookForm ID="BookForm" runat="server" />

                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Calcel" OnClick="btnCancel_Click" />
            </div>
        </section>
    </main>
</asp:Content>