using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoinQueries
{
    /// <summary>
    /// Form that displays output from database queries of the AuthorISBN table
    /// in 3 different formats
    /// 
    /// 1. Get a list of all the titles and the authors who wrote them. Sort the results by title.
    /// 
    /// 2. Get a list of all the titles and the authors who wrote them. Sort the results by title.
    ///    For each title sort the authors alphabetically by last name, then first name.
    ///   
    /// 3. Get a list of all the authors grouped by title, sorted by title; 
    ///    For a given title sort the author names alphabetically by last name then first name.
    /// </summary>
    public partial class JoiningTableData : Form
    {
        public JoiningTableData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads list of all authors and their related titles and formats the output text in 3 different ways
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JoiningTableData_Load(object sender, EventArgs e)
        {
            var dbcontext = new BooksExamples.BooksEntities();

            var authorsTitles =
                from author in dbcontext.Authors
                from book in author.Titles
                orderby book.Title1
                select new {
                    author.FirstName,
                    author.LastName,
                    book.Title1
                };

            //Output for part 1
            outputText.AppendText("Authors and Titles:");
            foreach (var row in authorsTitles)
            {
                outputText.AppendText($"\r\n\t{row.FirstName,-8} {row.LastName,-8} {row.Title1,-8}");
            }

            var authorsTitlesSort =
                from book in dbcontext.Titles
                from author in book.Authors
                orderby book.Title1, author.LastName, author.FirstName
                select new {
                    author.FirstName,
                    author.LastName,
                    book.Title1
                };

            //Output for part 2
            outputText.AppendText("\r\n\r\nAuthors and Titles (Sorted by Name):");
            foreach (var row in authorsTitlesSort)
            {
                outputText.AppendText($"\r\n\t{row.FirstName,-8} {row.LastName,-8} {row.Title1}");
            }

            var bookAuthorList =
                from book in dbcontext.Titles
                orderby book.Title1 
                select new {
                    Title = book.Title1,
                    authorName = from author in book.Authors
                    orderby author.LastName, author.FirstName
                    select author.FirstName + " " + author.LastName
                };

            //Output for part 3
            outputText.AppendText("\r\n\r\nTitle List with Associated Authors (Sorted by Name):");
            foreach (var book in bookAuthorList)
            {
                //Display Book's name
                outputText.AppendText($"\n\r\n\t{book.Title}:\n");

                //Display authors who worked on the title
                foreach (var author in book.authorName)
                {
                    outputText.AppendText($"\r\n\t\t{author}");
                }
            }
        }
    }
}
