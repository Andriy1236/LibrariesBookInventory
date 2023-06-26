using LibrariesBookInventory.Application.Book.Delete;
using MediatR;
using System;
using System.Linq;
using System.Web.UI;

namespace LibrariesBookInventory
{
    public partial class DeleteBook : Page
    {
        public IMediator Mediator { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void BtnYes_Click(object sender, EventArgs e)
        {
            var idParameter = Request.QueryString["Id"];
            if (string.IsNullOrEmpty(idParameter)) return;
            long.TryParse(idParameter,out var id);
            var result = await Mediator.Send(new DeleteBookCommand() { Id = id });

            if (!(result.Errors != null && result.Errors.Any()))
                Response.Redirect("Home.aspx", false);

            ErrorsListView.DataSource = result.Errors;
            ErrorsListView.DataBind();
            ErrorsListView.Visible = true;
        }
        protected void BtnNo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx", false);

        }
    }
}