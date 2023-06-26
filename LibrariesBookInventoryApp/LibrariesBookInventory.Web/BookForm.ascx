<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookForm.ascx.cs" Inherits="LibrariesBookInventory.BookForm"%>

<asp:ListView runat="server" ID="ErrorsListView" Visible="false">
    <ItemTemplate>
        <div class="error-item">
            <span style="background-color: red" class="error-message"><%# Eval("ErrorMessage") %></span>
        </div>
    </ItemTemplate>
</asp:ListView>

<div class="book-form">
    <div class="form-row">
        <label for="Title">Title:</label>
        <asp:TextBox ID="TitleTextBox" runat="server"></asp:TextBox>
    </div>

    <div class="form-row">
        <label for="Author">Author:</label>
        <asp:TextBox ID="AuthorTextBox" runat="server"></asp:TextBox>
    </div>

    <div class="form-row">
        <label for="ISBN">ISBN:</label>
        <asp:TextBox ID="ISBNTextBox" runat="server"></asp:TextBox>
    </div>

    <div class="form-row">
        <label for="PublicationYear">PublicationYear:</label>
        <asp:TextBox ID="PublicationYearTextBox" runat="server" type="number"></asp:TextBox>
    </div>

    <div class="form-row">
        <label for="Quantity">Quantity:</label>
        <asp:TextBox ID="QuantityTextBox" runat="server" type="number"></asp:TextBox>
    </div>

    <div class="form-row">
        <label for="CategoryDropdown">Category:</label>
        <asp:DropDownList ID="CategoryDropdown" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
    </div>
</div>
