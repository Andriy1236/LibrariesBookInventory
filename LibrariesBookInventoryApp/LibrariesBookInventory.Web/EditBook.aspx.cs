using LibrariesBookInventory.Application.Book.Get;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Web.UI;
using LibrariesBookInventory.Application.Book.Update;

namespace LibrariesBookInventory
{
    public partial class EditBook : Page
    {
        public IMediator Mediator { get; set; }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var idParameter = Request.QueryString["Id"];
                
                if (string.IsNullOrEmpty(idParameter)) return;
                
                long.TryParse(idParameter, out var id);
                var book = (await Mediator.Send(new GetBookQuery() { Id = id}, new CancellationToken())).Data;

                BookForm.Title = book.Title;
                BookForm.Author = book.Author;
                BookForm.ISBN = book.ISBN;
                BookForm.PublicationYear = book.PublicationYear.ToString();
                BookForm.Quantity = book.Quantity.ToString();
            }
        }

        protected async void btnUpdate_Click(object sender, EventArgs e)
        {
            var idParameter = Request.QueryString["Id"];
            long.TryParse(idParameter, out var id);

            var updateBookCommand = new UpdateBookCommand
            {
                Id = id,
                Title = BookForm.Title,
                Author = BookForm.Author,
                ISBN = BookForm.ISBN,
                PublicationYear = int.Parse(BookForm.PublicationYear),
                Quantity = int.Parse(BookForm.Quantity),
                CategoryId = int.Parse(BookForm.Category.Items[BookForm.Category.SelectedIndex].Value)
            };


            var result = await Mediator.Send(updateBookCommand);

            if (!(result.Errors != null && result.Errors.Any()))
                Response.Redirect("Home.aspx", false);

            BookForm.ErrorsList.DataSource = result.Errors;
            BookForm.ErrorsList.DataBind();
            BookForm.ErrorsList.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx", false);
        }
    }
}