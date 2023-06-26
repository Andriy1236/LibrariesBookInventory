<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"  CodeBehind="~/DeleteBook.aspx.cs" Inherits="LibrariesBookInventory.DeleteBook"  Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="col-md-12">
            <div>
            <asp:ListView runat="server" ID="ErrorsListView" Visible = "false">
                <ItemTemplate>
                    <div class="error-item">
                        <span style="background-color:red" class="error-message"><%# Eval("ErrorMessage") %></span>
                    </div>
                </ItemTemplate>
            </asp:ListView>
                <h6>Are you really want to delete this book?</h6>
                <div>
                <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="BtnYes_Click" />
                <asp:Button ID="btnNo" runat="server" Text="No" OnClick="BtnNo_Click" />
                </div>
            </div>
        </section>
    </main>
</asp:Content>