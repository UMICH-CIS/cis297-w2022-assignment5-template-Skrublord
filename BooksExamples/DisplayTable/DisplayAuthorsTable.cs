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

namespace DisplayTable
{
    /// <summary>
    /// Tells form what to do on certain actions made on the form
    /// </summary>
    public partial class DisplayAuthorsTable : Form
    {
        //Boolean value to lock search so query can be performed, remember to unlock after
        private bool searchBool = true;

        /// <summary>
        /// Initializes DisplayTable
        /// </summary>
        public DisplayAuthorsTable()
        {
            InitializeComponent();
        }

        //Entity Framework DbContext
        private BooksExamples.BooksEntities dbcontext = new BooksExamples.BooksEntities();

        /// <summary>
        /// load data from database into DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayAuthorsTable_Load(object sender, EventArgs e)
        {
            if (searchBool)
            {
                //load Authors table ordered by LastName then FirstName
                dbcontext.Authors
                    .OrderBy(author => author.LastName)
                    .ThenBy(author => author.FirstName)
                    .Load();
                //specify datasource for authorBindingSource
                authorBindingSource.DataSource = dbcontext.Authors.Local;
            }
        
        }

        /// <summary>
        /// Refreshes the list of authors that is displayed in the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void authorBindingNavigator_RefreshItems(object sender, EventArgs e)
        {
            if (searchBool)
            {
                //load Authors table ordered by LastName then FirstName
                dbcontext.Authors
                    .OrderBy(author => author.LastName)
                    .ThenBy(author => author.FirstName)
                    .Load();
                //specify datasource for authorBindingSource
                authorBindingSource.DataSource = dbcontext.Authors.Local;
            }
        }
        
        /// <summary>
        /// Saves added items to table into the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void authorBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            authorBindingSource.EndEdit();
            try
            {
                dbcontext.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                MessageBox.Show("FirstName and LastName must contain values", "Entity Validation Exception");
            }
        }

        /// <summary>
        /// Performs a search based on the text entered in the NameInput text box for a matching lastname in the database
        /// Sets the searchBool to false so it will not auto-refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            searchBool = false;
            authorBindingSource.DataSource = dbcontext.Authors.Local
                .Where(author => author.LastName.StartsWith(NameInput.Text))
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName);
            authorBindingSource.MoveFirst();
        }

        /// <summary>
        /// Resets the searchBool to true to re-enable auto-refresh
        /// then refreshes list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object sender, EventArgs e)
        {
            searchBool = true;
            authorBindingNavigator_RefreshItems(sender, e);
        }
    }
}
