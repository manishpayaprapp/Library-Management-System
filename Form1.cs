namespace LibraryManagementSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private List<Book> books = new();

    public Form1()
    {
        InitializeComponent();
        InitializeSampleData();
        ConfigureGrid();
        BindGrid(books);
        RefreshDashboard();
    }

    private void InitializeSampleData()
    {
        books = new List<Book>
        {
            new Book(1, "Dune", "Frank Herbert", "978-0-441-17271-9", new DateTime(1965,8,1), "Available"),
            new Book(2, "Frankenstein", "Mary Shelley", "978-0-14-143947-1", new DateTime(1818,1,1), "Available"),
            new Book(3, "The Foundation", "Isaac Asimov", "978-0-553-29335-7", new DateTime(1951,5,1), "Active Loan"),
            new Book(4, "Dracula", "Bram Stoker", "978-0-14-143984-6", new DateTime(1897,5,26), "Available"),
            new Book(5, "Neuromancer", "William Gibson", "978-0-441-56959-5", new DateTime(1984,7,1), "Available"),
            new Book(6, "Hyperion", "Dan Simmons", "978-0-553-28368-6", new DateTime(1989,12,1), "Active Loan")
        };
    }

    private void ConfigureGrid()
    {
        itemsGrid.Columns.Clear();
        itemsGrid.Columns.Add("No", "#");
        itemsGrid.Columns.Add("Title", "Title");
        itemsGrid.Columns.Add("Author", "Author");
        itemsGrid.Columns.Add("Accession", "Accession ID");
        itemsGrid.Columns.Add("Published", "Published");
        itemsGrid.Columns.Add("Status", "Status");
    }

    private void BindGrid(IEnumerable<Book> data)
    {
        itemsGrid.Rows.Clear();
        foreach (var b in data)
        {
            itemsGrid.Rows.Add(b.No, b.Title, b.Author, b.AccessionId, b.Published.ToString("MMM d, yyyy"), b.Status);
        }
        RefreshDashboard();
    }

    private void RefreshDashboard()
    {
        lblItemsCount.Text = books.Count.ToString();
        lblReadyCount.Text = books.Count(b => b.Status == "Available").ToString();
        lblLoansCount.Text = books.Count(b => b.Status.Contains("Loan")).ToString();
        lblMembersCount.Text = "1245"; // placeholder
    }

    private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        var q = searchTextBox.Text.Trim();
        if (string.IsNullOrEmpty(q))
        {
            BindGrid(books);
            return;
        }
        var filtered = books.Where(b => b.Title.Contains(q, StringComparison.OrdinalIgnoreCase)
            || b.Author.Contains(q, StringComparison.OrdinalIgnoreCase)
            || b.AccessionId.Contains(q, StringComparison.OrdinalIgnoreCase));
        BindGrid(filtered);
    }

    private void BtnRegister_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Register Item clicked — implement registration form here.", "Register");
    }

    private void BtnOverview_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Overview clicked — this is the dashboard.", "Overview");
    }

    private void BtnCollection_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Collection clicked — show collection view.", "Collection");
    }

    private void BtnAddItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Add Item clicked — open add item dialog.", "Add Item");
    }

    private void BtnLoans_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Loans clicked — show loans.", "Loans");
    }

    private void BtnMembers_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Members clicked — show members.", "Members");
    }

    private void BtnReports_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Reports clicked — show reports.", "Reports");
    }

    private void BtnSettings_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Settings clicked — show settings.", "Settings");
    }

    private record Book(int No, string Title, string Author, string AccessionId, DateTime Published, string Status);
}
