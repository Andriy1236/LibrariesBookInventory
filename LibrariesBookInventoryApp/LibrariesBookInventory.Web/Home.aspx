<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="LibrariesBookInventory.Home"  Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="row">
            <section class="col-md-12">
                <asp:Button ID="BtnCreate" runat="server" Text="Create Book" onclick="BtnCreate_OnClick" />
                <div>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowEditing="GridView1_RowEditing" OnRowDeleting="GridView1_RowDeleting" DataKeyNames="id">
                        <Columns>
                            <asp:BoundField HeaderText="Id" DataField="Id" Visible="false" ItemStyle-CssClass="center-align" HeaderStyle-CssClass="center-align" />
                            <asp:BoundField HeaderText="Title" DataField="Title" ItemStyle-CssClass="center-align" HeaderStyle-CssClass="center-align" />
                            <asp:BoundField HeaderText="Author" DataField="Author" ItemStyle-CssClass="center-align" HeaderStyle-CssClass="center-align" />
                            <asp:BoundField HeaderText="ISBN" DataField="ISBN" ItemStyle-CssClass="center-align" HeaderStyle-CssClass="center-align" />
                            <asp:BoundField HeaderText="PublicationYear" DataField="PublicationYear" ItemStyle-CssClass="center-align" HeaderStyle-CssClass="center-align" />
                            <asp:BoundField HeaderText="Quantity" DataField="Quantity" ItemStyle-CssClass="center-align" HeaderStyle-CssClass="center-align" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:Button ID="BtnDelete" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="center-align" />
                    </asp:GridView>
                </div>
            </section>
        </div>
    </main>
</asp:Content>
