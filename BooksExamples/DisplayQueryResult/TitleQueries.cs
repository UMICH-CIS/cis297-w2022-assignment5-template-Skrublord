using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayQueryResult
{
    /// <summary>
    /// Form that accesses the database for ISBN, Title, EditionNumber, and CopyrightYear
    /// allows the user to search by book titles.
    /// </summary>
    public partial class TitleQueries : Form
    {
        /// <summary>
        /// Initializes TitleQueries
        /// </summary>
        public TitleQueries()
        {
            InitializeComponent();
        }

        private BooksExamples.BooksEntities dbcontext = new BooksExamples.BooksEntities();

        /// <summary>
        /// Loads the list of all titles in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleQueries_Load(object sender, EventArgs e)
        {
            dbcontext.Titles
                .OrderBy(titles => titles.Title1).Load();

            titleBindingSource.DataSource = dbcontext.Titles.Local;
        }

        /// <summary>
        /// Performs a search in the database for any book title that contains the input text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchbutton_Click(object sender, EventArgs e)
        {

            titleBindingSource.DataSource = dbcontext.Titles.Local
                .Where(title => title.Title1.Contains(TitleInput.Text))
                .OrderBy(title => title.Title1);

            titleBindingSource.MoveFirst();
        }

        /// <summary>
        /// Reloads search so all results show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetbutton_Click(object sender, EventArgs e)
        {
            TitleQueries_Load(sender, e);
        }
    }
}
