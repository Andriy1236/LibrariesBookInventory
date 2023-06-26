using LibrariesBookInventory.Application.Category.GetAll;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibrariesBookInventory
{
    public partial class BookForm : UserControl
    {
        public IMediator Mediator { get; set; }
        public string Title
        {
            get { return TitleTextBox.Text; }
            set { TitleTextBox.Text = value; }
        }
        public string Author
        {
            get { return AuthorTextBox.Text; }
            set { AuthorTextBox.Text = value; }
        }
        public string Quantity
        {
            get { return QuantityTextBox.Text; }
            set { QuantityTextBox.Text = value; }
        }
        public string ISBN
        {
            get { return ISBNTextBox.Text; }
            set { ISBNTextBox.Text = value; }
        }
        public string PublicationYear
        {
            get { return PublicationYearTextBox.Text; }
            set { PublicationYearTextBox.Text = value; }
        }
        public DropDownList Category
        {
            get { return CategoryDropdown; }
        }
        public ListView ErrorsList
        {
            get { return ErrorsListView; }
            set { ErrorsListView = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var categories = Task.Run(() => Mediator.Send(new GetAllCategoriesQuery())).Result;

                CategoryDropdown.DataSource = categories.Data;
                CategoryDropdown.DataBind();
            }
        }
    }
}