using LibrariesBookInventory.Application.Book.Create;
using MediatR;
using System;
using System.Linq;
using System.Web.UI;

namespace LibrariesBookInventory
{
    public partial class CreateBook : Page
    {
        public IMediator Mediator { get; set; }
        public CreateBookValidator CreateBookValidator { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void BtnCreate_Click(object sender, EventArgs e)
        {
            CreateBookCommand createBookCommand = new CreateBookCommand
            {
                Title = BookForm.Title,
                Author = BookForm.Author,
                ISBN = BookForm.ISBN,
                PublicationYear = int.Parse(BookForm.PublicationYear),
                Quantity = int.Parse(BookForm.Quantity),
                CategoryId = int.Parse(BookForm.Category.Items[BookForm.Category.SelectedIndex].Value)
            };

            var result = await Mediator.Send(createBookCommand);

            if (!(result.Errors != null && result.Errors.Any()))
                Response.Redirect("Home.aspx", false);

            BookForm.ErrorsList.DataSource = result.Errors;
            BookForm.ErrorsList.DataBind();
            BookForm.ErrorsList.Visible = true;
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx", false);
        }
    }
}