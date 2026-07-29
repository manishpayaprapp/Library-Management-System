using Npgsql;

namespace LibraryManagementSystem.Web.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public NpgsqlConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public async Task InitializeSchemaAsync()
        {
            await using var conn = GetConnection();
            await conn.OpenAsync();

            // Create tables only if they don't exist — preserves all existing data across restarts
            var sql = @"
                CREATE TABLE IF NOT EXISTS users (
                    id SERIAL PRIMARY KEY,
                    username VARCHAR(100) NOT NULL UNIQUE,
                    password_hash VARCHAR(255) NOT NULL,
                    full_name VARCHAR(200) NOT NULL,
                    role VARCHAR(50) NOT NULL DEFAULT 'Librarian',
                    created_at TIMESTAMPTZ DEFAULT NOW()
                );

                CREATE TABLE IF NOT EXISTS books (
                    id SERIAL PRIMARY KEY,
                    title VARCHAR(500) NOT NULL,
                    author VARCHAR(300) NOT NULL,
                    accession_id VARCHAR(100) NOT NULL UNIQUE,
                    published DATE NOT NULL,
                    genre VARCHAR(100),
                    status VARCHAR(50) NOT NULL DEFAULT 'Available',
                    created_at TIMESTAMPTZ DEFAULT NOW()
                );

                CREATE TABLE IF NOT EXISTS members (
                    id SERIAL PRIMARY KEY,
                    full_name VARCHAR(200) NOT NULL,
                    email VARCHAR(200) NOT NULL UNIQUE,
                    phone VARCHAR(30),
                    membership_date DATE NOT NULL DEFAULT CURRENT_DATE,
                    status VARCHAR(50) NOT NULL DEFAULT 'Active',
                    created_at TIMESTAMPTZ DEFAULT NOW()
                );

                CREATE TABLE IF NOT EXISTS loans (
                    id SERIAL PRIMARY KEY,
                    book_id INT NOT NULL REFERENCES books(id),
                    member_id INT NOT NULL REFERENCES members(id),
                    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
                    due_date DATE NOT NULL,
                    return_date DATE,
                    status VARCHAR(50) NOT NULL DEFAULT 'Active',
                    created_at TIMESTAMPTZ DEFAULT NOW()
                );
            ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            // Open a fresh connection after pool clear to avoid stale schema cache
            await using var conn = GetConnection();
            await conn.OpenAsync();

            // Seed admin user
            var userCount = (long)(await new NpgsqlCommand("SELECT COUNT(*) FROM users", conn).ExecuteScalarAsync() ?? 0);
            if (userCount == 0)
            {
                var insertUser = @"
                    INSERT INTO users (username, password_hash, full_name, role) VALUES
                    ('admin', 'admin123', 'Admin', 'Librarian'),
                    ('librarian', 'lib123', 'Sarah Johnson', 'Librarian');
                ";
                await new NpgsqlCommand(insertUser, conn).ExecuteNonQueryAsync();
            }

            // Seed books
            var bookCount = (long)(await new NpgsqlCommand("SELECT COUNT(*) FROM books", conn).ExecuteScalarAsync() ?? 0);
            if (bookCount == 0)
            {
                var insertBooks = @"
                    INSERT INTO books (title, author, accession_id, published, genre, status) VALUES
                    ('Dune', 'Frank Herbert', '978-0-441-17271-9', '1965-08-01', 'Science Fiction', 'Available'),
                    ('Frankenstein', 'Mary Shelley', '978-0-14-143947-1', '1818-01-01', 'Gothic Fiction', 'Available'),
                    ('The Foundation', 'Isaac Asimov', '978-0-553-29335-7', '1951-05-01', 'Science Fiction', 'Active Loan'),
                    ('Dracula', 'Bram Stoker', '978-0-14-143984-6', '1897-05-26', 'Horror', 'Available'),
                    ('Neuromancer', 'William Gibson', '978-0-441-56959-5', '1984-07-01', 'Cyberpunk', 'Reserved'),
                    ('Hyperion', 'Dan Simmons', '978-0-553-28368-6', '1989-12-01', 'Science Fiction', 'Available'),
                    ('Pride and Prejudice', 'Jane Austen', '978-0-14-143951-8', '1813-01-28', 'Classic', 'Available'),
                    ('The Left Hand of Darkness', 'Ursula K. Le Guin', '978-0-441-47812-5', '1969-03-01', 'Science Fiction', 'Active Loan'),
                    ('Fahrenheit 451', 'Ray Bradbury', '978-0-345-34296-6', '1953-10-19', 'Dystopian Fiction', 'Active Loan'),
                    ('Great Expectations', 'Charles Dickens', '978-0-14-143956-3', '1861-08-01', 'Classic', 'Active Loan'),
                    ('A Game of Thrones', 'George R.R. Martin', '978-0-553-57340-4', '1996-08-01', 'Fantasy', 'Active Loan'),
                    ('The Hitchhiker''s Guide to the Galaxy', 'Douglas Adams', '978-0-345-39180-3', '1979-10-12', 'Comedy Science Fiction', 'Available');
                ";
                await new NpgsqlCommand(insertBooks, conn).ExecuteNonQueryAsync();
            }

            // Seed members — matches the Members page screenshot exactly
            var memberCount = (long)(await new NpgsqlCommand("SELECT COUNT(*) FROM members", conn).ExecuteScalarAsync() ?? 0);
            if (memberCount == 0)
            {
                var insertMembers = @"
                    INSERT INTO members (full_name, email, phone, membership_date, status) VALUES
                    ('Aarav Mehta', 'aarav.mehta@mailbox.com', '+91-9876543210', '2025-01-15', 'Suspended'),
                    ('Priya Nair', 'priya.nair@mailbox.com', '+91-9876543211', '2025-02-20', 'Active'),
                    ('Kabir Shah', 'kabir.shah@mailbox.com', '+91-9876543212', '2025-03-10', 'Active'),
                    ('Ishata Rao', 'ishata.rao@mailbox.com', '+91-9876543213', '2025-03-25', 'Active'),
                    ('Rohan Verma', 'rohan.verma@mailbox.com', '+91-9876543214', '2025-04-05', 'Active'),
                    ('Sana Iqbal', 'sana.iqbal@mailbox.com', '+91-9876543215', '2025-04-18', 'Active'),
                    ('Devansh Gupta', 'devansh.gupta@mailbox.com', '+91-9876543216', '2025-05-01', 'Active'),
                    ('Meera Pillai', 'meera.pillai@mailbox.com', '+91-9876543217', '2025-05-15', 'Suspended'),
                    ('Aditya Kulkarni', 'aditya.kulkarni@mailbox.com', '+91-9876543218', '2025-06-01', 'Active'),
                    ('Neha Joshi', 'neha.joshi@mailbox.com', '+91-9876543219', '2025-06-20', 'Active'),
                    ('Farhan Ali', 'farhan.ali@mailbox.com', '+91-9876543220', '2025-07-01', 'Active'),
                    ('Tanvi Deshmukh', 'tanvi.deshmukh@mailbox.com', '+91-9876543221', '2025-07-10', 'Active');
                ";
                await new NpgsqlCommand(insertMembers, conn).ExecuteNonQueryAsync();
            }

            // Seed loans — matches Active Loans page screenshot
            var loanCount = (long)(await new NpgsqlCommand("SELECT COUNT(*) FROM loans", conn).ExecuteScalarAsync() ?? 0);
            if (loanCount == 0)
            {
                var insertLoans = @"
                    INSERT INTO loans (book_id, member_id, issue_date, due_date, return_date, status) VALUES
                    (8,  8,  '2026-07-12', '2026-07-26', NULL, 'Active'),
                    (9,  6,  '2026-07-12', '2026-07-26', NULL, 'Active'),
                    (3,  3,  '2026-07-17', '2026-07-31', NULL, 'Active'),
                    (10, 4,  '2026-07-17', '2026-07-31', NULL, 'Active'),
                    (11, 2,  '2026-07-17', '2026-07-31', NULL, 'Active'),
                    (1,  7,  '2026-06-01', '2026-06-15', '2026-06-14', 'Returned'),
                    (2,  5,  '2026-06-10', '2026-06-24', '2026-06-22', 'Returned'),
                    (7,  9,  '2026-06-15', '2026-06-29', '2026-06-28', 'Returned');
                ";
                await new NpgsqlCommand(insertLoans, conn).ExecuteNonQueryAsync();
            }
        }

    }
}
