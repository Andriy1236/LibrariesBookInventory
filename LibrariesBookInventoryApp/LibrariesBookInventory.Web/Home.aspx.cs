using LibrariesBookInventory.Application.Book.GetAll;
using MediatR;
using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibrariesBookInventory
{
    public partial class Home : Page
    {
        public IMediator Mediator { get; set; }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var books = await Mediator.Send(new GetAllBooksQuery(), new CancellationToken());
                GridView1.DataSource = books.Data;
                GridView1.DataBind();
            }
        }

        protected void BtnCreate_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("CreateBook", false);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var itemId = Convert.ToInt32(GridView1.DataKeys[e.NewEditIndex]?.Value);
            Response.Redirect("EditBook.aspx?Id=" + itemId, false);
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int itemId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex]?.Value);
            Response.Redirect("DeleteBook.aspx?Id=" + itemId, false);
        }
    }
}