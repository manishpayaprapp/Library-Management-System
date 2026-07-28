namespace LibraryManagementSystem;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private Panel sidebarPanel;
    private Button btnOverview;
    private Button btnCollection;
    private Button btnAddItem;
    private Button btnLoans;
    private Button btnMembers;
    private Button btnReports;
    private Button btnSettings;
    private Panel headerPanel;
    private TextBox searchTextBox;
    private Button btnRegister;
    private Label titleLabel;
    private Panel cardsPanel;
    private Panel cardItems;
    private Label lblItemsCount;
    private Label lblItemsTitle;
    private Panel cardReady;
    private Label lblReadyCount;
    private Label lblReadyTitle;
    private Panel cardLoans;
    private Label lblLoansCount;
    private Label lblLoansTitle;
    private Panel cardMembers;
    private Label lblMembersCount;
    private Label lblMembersTitle;
    private Panel mainPanel;
    private DataGridView itemsGrid;
    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        sidebarPanel = new Panel();
        btnOverview = new Button();
        btnCollection = new Button();
        btnAddItem = new Button();
        btnLoans = new Button();
        btnMembers = new Button();
        btnReports = new Button();
        btnSettings = new Button();
        headerPanel = new Panel();
        searchTextBox = new TextBox();
        btnRegister = new Button();
        titleLabel = new Label();
        cardsPanel = new Panel();
        cardItems = new Panel();
        lblItemsCount = new Label();
        lblItemsTitle = new Label();
        cardReady = new Panel();
        lblReadyCount = new Label();
        lblReadyTitle = new Label();
        cardLoans = new Panel();
        lblLoansCount = new Label();
        lblLoansTitle = new Label();
        cardMembers = new Panel();
        lblMembersCount = new Label();
        lblMembersTitle = new Label();
        mainPanel = new Panel();
        itemsGrid = new DataGridView();
        SuspendLayout();
        // 
        // sidebarPanel
        // 
        sidebarPanel.BackColor = Color.FromArgb(18, 61, 103);
        sidebarPanel.Dock = DockStyle.Left;
        sidebarPanel.Width = 180;
        sidebarPanel.Padding = new Padding(10);
        sidebarPanel.Controls.Add(btnSettings);
        sidebarPanel.Controls.Add(btnReports);
        sidebarPanel.Controls.Add(btnMembers);
        sidebarPanel.Controls.Add(btnLoans);
        sidebarPanel.Controls.Add(btnAddItem);
        sidebarPanel.Controls.Add(btnCollection);
        sidebarPanel.Controls.Add(btnOverview);
        // 
        // btnOverview
        // 
        btnOverview.Dock = DockStyle.Top;
        btnOverview.Height = 45;
        btnOverview.FlatStyle = FlatStyle.Flat;
        btnOverview.ForeColor = Color.White;
        btnOverview.Text = "Overview";
        btnOverview.TextAlign = ContentAlignment.MiddleLeft;
        btnOverview.Padding = new Padding(12, 0, 0, 0);
        btnOverview.Click += BtnOverview_Click;
        // 
        // btnCollection
        // 
        btnCollection.Dock = DockStyle.Top;
        btnCollection.Height = 45;
        btnCollection.FlatStyle = FlatStyle.Flat;
        btnCollection.ForeColor = Color.White;
        btnCollection.Text = "Collection";
        btnCollection.TextAlign = ContentAlignment.MiddleLeft;
        btnCollection.Padding = new Padding(12, 0, 0, 0);
        btnCollection.Click += BtnCollection_Click;
        // 
        // btnAddItem
        // 
        btnAddItem.Dock = DockStyle.Top;
        btnAddItem.Height = 45;
        btnAddItem.FlatStyle = FlatStyle.Flat;
        btnAddItem.ForeColor = Color.White;
        btnAddItem.Text = "Add Item";
        btnAddItem.TextAlign = ContentAlignment.MiddleLeft;
        btnAddItem.Padding = new Padding(12, 0, 0, 0);
        btnAddItem.Click += BtnAddItem_Click;
        // 
        // btnLoans
        // 
        btnLoans.Dock = DockStyle.Top;
        btnLoans.Height = 45;
        btnLoans.FlatStyle = FlatStyle.Flat;
        btnLoans.ForeColor = Color.White;
        btnLoans.Text = "Loans";
        btnLoans.TextAlign = ContentAlignment.MiddleLeft;
        btnLoans.Padding = new Padding(12, 0, 0, 0);
        btnLoans.Click += BtnLoans_Click;
        // 
        // btnMembers
        // 
        btnMembers.Dock = DockStyle.Top;
        btnMembers.Height = 45;
        btnMembers.FlatStyle = FlatStyle.Flat;
        btnMembers.ForeColor = Color.White;
        btnMembers.Text = "Members";
        btnMembers.TextAlign = ContentAlignment.MiddleLeft;
        btnMembers.Padding = new Padding(12, 0, 0, 0);
        btnMembers.Click += BtnMembers_Click;
        // 
        // btnReports
        // 
        btnReports.Dock = DockStyle.Top;
        btnReports.Height = 45;
        btnReports.FlatStyle = FlatStyle.Flat;
        btnReports.ForeColor = Color.White;
        btnReports.Text = "Reports";
        btnReports.TextAlign = ContentAlignment.MiddleLeft;
        btnReports.Padding = new Padding(12, 0, 0, 0);
        btnReports.Click += BtnReports_Click;
        // 
        // btnSettings
        // 
        btnSettings.Dock = DockStyle.Bottom;
        btnSettings.Height = 45;
        btnSettings.FlatStyle = FlatStyle.Flat;
        btnSettings.ForeColor = Color.White;
        btnSettings.Text = "Settings";
        btnSettings.TextAlign = ContentAlignment.MiddleLeft;
        btnSettings.Padding = new Padding(12, 0, 0, 0);
        btnSettings.Click += BtnSettings_Click;
        // 
        // headerPanel
        // 
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 72;
        headerPanel.Padding = new Padding(16);
        headerPanel.BackColor = Color.White;
        headerPanel.Controls.Add(searchTextBox);
        headerPanel.Controls.Add(btnRegister);
        headerPanel.Controls.Add(titleLabel);
        // 
        // titleLabel
        // 
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        titleLabel.ForeColor = Color.FromArgb(34, 45, 67);
        titleLabel.Location = new Point(200, 20);
        titleLabel.Text = "Library Management System";
        // 
        // searchTextBox
        // 
        searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        searchTextBox.Width = 380;
        searchTextBox.Location = new Point(520, 20);
        searchTextBox.PlaceholderText = "Search books, authors, or ISBN...";
        searchTextBox.KeyUp += SearchTextBox_KeyUp;
        // 
        // btnRegister
        // 
        btnRegister.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRegister.Text = "Register Item";
        btnRegister.Width = 120;
        btnRegister.Height = 32;
        btnRegister.Location = new Point(920, 20);
        btnRegister.BackColor = Color.FromArgb(13, 110, 253);
        btnRegister.ForeColor = Color.White;
        btnRegister.FlatStyle = FlatStyle.Flat;
        btnRegister.Click += BtnRegister_Click;
        // 
        // cardsPanel
        // 
        cardsPanel.Dock = DockStyle.Top;
        cardsPanel.Height = 120;
        cardsPanel.Padding = new Padding(16);
        cardsPanel.BackColor = Color.WhiteSmoke;
        cardsPanel.Controls.Add(cardMembers);
        cardsPanel.Controls.Add(cardLoans);
        cardsPanel.Controls.Add(cardReady);
        cardsPanel.Controls.Add(cardItems);
        // 
        // cardItems
        // 
        cardItems.Width = 220;
        cardItems.Height = 80;
        cardItems.BackColor = Color.White;
        cardItems.Padding = new Padding(12);
        cardItems.Location = new Point(200, 10);
        cardItems.BorderStyle = BorderStyle.FixedSingle;
        cardItems.Controls.Add(lblItemsCount);
        cardItems.Controls.Add(lblItemsTitle);
        // 
        // lblItemsCount
        // 
        lblItemsCount.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        lblItemsCount.Text = "0";
        lblItemsCount.AutoSize = true;
        lblItemsCount.Location = new Point(12, 6);
        // 
        // lblItemsTitle
        // 
        lblItemsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblItemsTitle.Text = "Items in Repository";
        lblItemsTitle.Location = new Point(12, 48);
        // 
        // cardReady
        // 
        cardReady.Width = 220;
        cardReady.Height = 80;
        cardReady.BackColor = Color.White;
        cardReady.Padding = new Padding(12);
        cardReady.Location = new Point(440, 10);
        cardReady.BorderStyle = BorderStyle.FixedSingle;
        cardReady.Controls.Add(lblReadyCount);
        cardReady.Controls.Add(lblReadyTitle);
        // 
        // lblReadyCount
        // 
        lblReadyCount.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        lblReadyCount.Text = "0";
        lblReadyCount.AutoSize = true;
        lblReadyCount.Location = new Point(12, 6);
        // 
        // lblReadyTitle
        // 
        lblReadyTitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblReadyTitle.Text = "Ready for Request";
        lblReadyTitle.Location = new Point(12, 48);
        // 
        // cardLoans
        // 
        cardLoans.Width = 220;
        cardLoans.Height = 80;
        cardLoans.BackColor = Color.White;
        cardLoans.Padding = new Padding(12);
        cardLoans.Location = new Point(680, 10);
        cardLoans.BorderStyle = BorderStyle.FixedSingle;
        cardLoans.Controls.Add(lblLoansCount);
        cardLoans.Controls.Add(lblLoansTitle);
        // 
        // lblLoansCount
        // 
        lblLoansCount.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        lblLoansCount.Text = "0";
        lblLoansCount.AutoSize = true;
        lblLoansCount.Location = new Point(12, 6);
        // 
        // lblLoansTitle
        // 
        lblLoansTitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblLoansTitle.Text = "Active Loans";
        lblLoansTitle.Location = new Point(12, 48);
        // 
        // cardMembers
        // 
        cardMembers.Width = 220;
        cardMembers.Height = 80;
        cardMembers.BackColor = Color.White;
        cardMembers.Padding = new Padding(12);
        cardMembers.Location = new Point(920, 10);
        cardMembers.BorderStyle = BorderStyle.FixedSingle;
        cardMembers.Controls.Add(lblMembersCount);
        cardMembers.Controls.Add(lblMembersTitle);
        // 
        // lblMembersCount
        // 
        lblMembersCount.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        lblMembersCount.Text = "0";
        lblMembersCount.AutoSize = true;
        lblMembersCount.Location = new Point(12, 6);
        // 
        // lblMembersTitle
        // 
        lblMembersTitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblMembersTitle.Text = "Registered Members";
        lblMembersTitle.Location = new Point(12, 48);
        // 
        // mainPanel
        // 
        mainPanel.Dock = DockStyle.Fill;
        mainPanel.Padding = new Padding(16);
        mainPanel.BackColor = Color.White;
        mainPanel.Controls.Add(itemsGrid);
        // 
        // itemsGrid
        // 
        itemsGrid.Dock = DockStyle.Fill;
        itemsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        itemsGrid.ReadOnly = true;
        itemsGrid.AllowUserToAddRows = false;
        itemsGrid.AllowUserToDeleteRows = false;
        itemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        itemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        // 
        // Form1
        // 
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 720);
        Controls.Add(mainPanel);
        Controls.Add(cardsPanel);
        Controls.Add(headerPanel);
        Controls.Add(sidebarPanel);
        Text = "Library Management System";
        ResumeLayout(false);
    }

    #endregion
}
