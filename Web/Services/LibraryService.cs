using Npgsql;

namespace LibraryManagementSystem.Web.Services
{
    // ─── Models ───────────────────────────────────────────────────────────────

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string AccessionId { get; set; } = string.Empty;
        public DateTime Published { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
    }

    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime MembershipDate { get; set; }
        public string Status { get; set; } = "Active";
        public int ActiveLoans { get; set; }
    }

    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "Active";
        public bool IsOverdue => ReturnDate == null && DueDate < DateTime.Today;
    }

    public class ReportSummary
    {
        public int TotalLoansThisMonth { get; set; }
        public int ActiveLoans { get; set; }
        public int OverdueLoans { get; set; }
        public int ReturnedThisMonth { get; set; }
        public List<PopularBook> PopularBooks { get; set; } = new();
        public List<Loan> RecentLoans { get; set; } = new();
    }

    public class PopularBook
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int LoanCount { get; set; }
    }

    // ─── Service ──────────────────────────────────────────────────────────────

    public class LibraryService
    {
        private readonly DatabaseService _db;

        public LibraryService(DatabaseService db) => _db = db;

        // ── Books ──────────────────────────────────────────────────────────

        public async Task<List<Book>> GetBooksAsync(string search = "")
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = string.IsNullOrWhiteSpace(search)
                ? "SELECT id, title, author, accession_id, published, COALESCE(genre,'') as genre, status FROM books ORDER BY id"
                : @"SELECT id, title, author, accession_id, published, COALESCE(genre,'') as genre, status FROM books
                    WHERE title ILIKE @q OR author ILIKE @q OR accession_id ILIKE @q
                    ORDER BY id";

            await using var cmd = new NpgsqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("q", $"%{search}%");

            var books = new List<Book>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                books.Add(MapBook(reader));
            return books;
        }

        public async Task<Book?> GetBookAsync(int id)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, title, author, accession_id, published, COALESCE(genre,'') as genre, status FROM books WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapBook(reader) : null;
        }

        public async Task AddBookAsync(Book book)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO books (title, author, accession_id, published, genre, status)
                        VALUES (@title, @author, @accid, @pub, @genre, 'Available')";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("title", book.Title);
            cmd.Parameters.AddWithValue("author", book.Author);
            cmd.Parameters.AddWithValue("accid", book.AccessionId);
            cmd.Parameters.AddWithValue("pub", DateOnly.FromDateTime(book.Published));
            cmd.Parameters.AddWithValue("genre", book.Genre ?? "");
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = @"UPDATE books SET title=@title, author=@author, accession_id=@accid,
                        published=@pub, genre=@genre, status=@status WHERE id=@id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("title", book.Title);
            cmd.Parameters.AddWithValue("author", book.Author);
            cmd.Parameters.AddWithValue("accid", book.AccessionId);
            cmd.Parameters.AddWithValue("pub", DateOnly.FromDateTime(book.Published));
            cmd.Parameters.AddWithValue("genre", book.Genre ?? "");
            cmd.Parameters.AddWithValue("status", book.Status);
            cmd.Parameters.AddWithValue("id", book.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM books WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ── Counts ─────────────────────────────────────────────────────────

        public async Task<int> GetTotalBooksAsync()
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            return (int)(long)(await new NpgsqlCommand("SELECT COUNT(*) FROM books", conn).ExecuteScalarAsync() ?? 0);
        }

        public async Task<int> GetAvailableBooksAsync()
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            return (int)(long)(await new NpgsqlCommand("SELECT COUNT(*) FROM books WHERE status='Available'", conn).ExecuteScalarAsync() ?? 0);
        }

        public async Task<int> GetActiveLoansCountAsync()
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            return (int)(long)(await new NpgsqlCommand("SELECT COUNT(*) FROM loans WHERE status IN ('Active','Overdue')", conn).ExecuteScalarAsync() ?? 0);
        }

        public async Task<int> GetTotalMembersAsync()
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            return (int)(long)(await new NpgsqlCommand("SELECT COUNT(*) FROM members", conn).ExecuteScalarAsync() ?? 0);
        }

        // ── Members ────────────────────────────────────────────────────────

        public async Task<List<Member>> GetMembersAsync(string search = "")
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = @"
                SELECT m.id, m.full_name, m.email, m.phone, m.membership_date, m.status,
                       COUNT(l.id) FILTER (WHERE l.status IN ('Active','Overdue')) as active_loans
                FROM members m
                LEFT JOIN loans l ON l.member_id = m.id";

            if (!string.IsNullOrWhiteSpace(search))
                sql += " WHERE m.full_name ILIKE @q OR m.email ILIKE @q OR m.phone ILIKE @q";

            sql += " GROUP BY m.id ORDER BY m.id";

            await using var cmd = new NpgsqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("q", $"%{search}%");

            var members = new List<Member>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                members.Add(MapMember(reader));
            return members;
        }

        public async Task AddMemberAsync(Member member)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO members (full_name, email, phone, membership_date, status)
                        VALUES (@name, @email, @phone, @date, 'Active')";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", member.FullName);
            cmd.Parameters.AddWithValue("email", member.Email);
            cmd.Parameters.AddWithValue("phone", member.Phone ?? "");
            cmd.Parameters.AddWithValue("date", DateOnly.FromDateTime(member.MembershipDate));
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateMemberAsync(Member member)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = "UPDATE members SET full_name=@name, email=@email, phone=@phone, status=@status WHERE id=@id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", member.FullName);
            cmd.Parameters.AddWithValue("email", member.Email);
            cmd.Parameters.AddWithValue("phone", member.Phone ?? "");
            cmd.Parameters.AddWithValue("status", member.Status);
            cmd.Parameters.AddWithValue("id", member.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteMemberAsync(int id)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM members WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ── Loans ──────────────────────────────────────────────────────────

        public async Task<List<Loan>> GetLoansAsync(string statusFilter = "")
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = @"
                SELECT l.id, l.book_id, b.title as book_title, l.member_id, m.full_name as member_name,
                       l.issue_date, l.due_date, l.return_date, l.status
                FROM loans l
                JOIN books b ON b.id = l.book_id
                JOIN members m ON m.id = l.member_id";

            if (!string.IsNullOrWhiteSpace(statusFilter))
                sql += " WHERE l.status = @status";

            sql += " ORDER BY l.id DESC";

            await using var cmd = new NpgsqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(statusFilter))
                cmd.Parameters.AddWithValue("status", statusFilter);

            var loans = new List<Loan>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                loans.Add(MapLoan(reader));
            return loans;
        }

        public async Task<List<Member>> GetMembersForSelectAsync()
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = "SELECT id, full_name, email, phone, membership_date, status, 0 as active_loans FROM members WHERE status='Active' ORDER BY full_name";
            await using var cmd = new NpgsqlCommand(sql, conn);
            var members = new List<Member>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                members.Add(MapMember(reader));
            return members;
        }

        public async Task<List<Book>> GetAvailableBooksForSelectAsync()
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var sql = "SELECT id, title, author, accession_id, published, COALESCE(genre,'') as genre, status FROM books WHERE status='Available' ORDER BY title";
            await using var cmd = new NpgsqlCommand(sql, conn);
            var books = new List<Book>();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                books.Add(MapBook(reader));
            return books;
        }

        public async Task IssueLoanAsync(int bookId, int memberId, DateTime dueDate)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            await using var tx = await conn.BeginTransactionAsync();
            await new NpgsqlCommand(
                $"INSERT INTO loans (book_id, member_id, issue_date, due_date, status) VALUES ({bookId}, {memberId}, CURRENT_DATE, @due, 'Active')",
                conn, tx) { Parameters = { new NpgsqlParameter("due", DateOnly.FromDateTime(dueDate)) } }
                .ExecuteNonQueryAsync();
            await new NpgsqlCommand($"UPDATE books SET status='Active Loan' WHERE id={bookId}", conn, tx).ExecuteNonQueryAsync();
            await tx.CommitAsync();
        }

        public async Task ReturnLoanAsync(int loanId, int bookId)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            await using var tx = await conn.BeginTransactionAsync();
            await new NpgsqlCommand($"UPDATE loans SET return_date=CURRENT_DATE, status='Returned' WHERE id={loanId}", conn, tx).ExecuteNonQueryAsync();
            await new NpgsqlCommand($"UPDATE books SET status='Available' WHERE id={bookId}", conn, tx).ExecuteNonQueryAsync();
            await tx.CommitAsync();
        }

        // ── Reports ────────────────────────────────────────────────────────

        public async Task<ReportSummary> GetReportSummaryAsync()
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            var summary = new ReportSummary();

            summary.TotalLoansThisMonth = (int)(long)(await new NpgsqlCommand(
                "SELECT COUNT(*) FROM loans WHERE DATE_TRUNC('month', issue_date) = DATE_TRUNC('month', CURRENT_DATE)", conn)
                .ExecuteScalarAsync() ?? 0);

            summary.ActiveLoans = (int)(long)(await new NpgsqlCommand(
                "SELECT COUNT(*) FROM loans WHERE status IN ('Active','Overdue')", conn)
                .ExecuteScalarAsync() ?? 0);

            summary.OverdueLoans = (int)(long)(await new NpgsqlCommand(
                "SELECT COUNT(*) FROM loans WHERE status='Overdue' OR (status='Active' AND due_date < CURRENT_DATE)", conn)
                .ExecuteScalarAsync() ?? 0);

            summary.ReturnedThisMonth = (int)(long)(await new NpgsqlCommand(
                "SELECT COUNT(*) FROM loans WHERE status='Returned' AND DATE_TRUNC('month', return_date) = DATE_TRUNC('month', CURRENT_DATE)", conn)
                .ExecuteScalarAsync() ?? 0);

            var popularSql = @"
                SELECT b.title, b.author, COUNT(l.id) as loan_count
                FROM loans l JOIN books b ON b.id=l.book_id
                GROUP BY b.id, b.title, b.author
                ORDER BY loan_count DESC LIMIT 5";
            await using var popCmd = new NpgsqlCommand(popularSql, conn);
            await using var popReader = await popCmd.ExecuteReaderAsync();
            while (await popReader.ReadAsync())
                summary.PopularBooks.Add(new PopularBook
                {
                    Title = popReader.GetString(0),
                    Author = popReader.GetString(1),
                    LoanCount = (int)popReader.GetInt64(2)
                });
            await popReader.CloseAsync();

            var recentSql = @"
                SELECT l.id, l.book_id, b.title, l.member_id, m.full_name,
                       l.issue_date, l.due_date, l.return_date, l.status
                FROM loans l
                JOIN books b ON b.id=l.book_id
                JOIN members m ON m.id=l.member_id
                ORDER BY l.id DESC LIMIT 10";
            await using var recentCmd = new NpgsqlCommand(recentSql, conn);
            await using var recentReader = await recentCmd.ExecuteReaderAsync();
            while (await recentReader.ReadAsync())
                summary.RecentLoans.Add(MapLoan(recentReader));

            return summary;
        }

        // ── Mappers ────────────────────────────────────────────────────────

        private static Book MapBook(NpgsqlDataReader r) => new Book
        {
            Id = r.GetInt32(0),
            Title = r.GetString(1),
            Author = r.GetString(2),
            AccessionId = r.GetString(3),
            Published = r.GetFieldValue<DateOnly>(4).ToDateTime(TimeOnly.MinValue),
            Genre = r.GetString(5),
            Status = r.GetString(6)
        };

        private static Member MapMember(NpgsqlDataReader r) => new Member
        {
            Id = r.GetInt32(0),
            FullName = r.GetString(1),
            Email = r.GetString(2),
            Phone = r.IsDBNull(3) ? "" : r.GetString(3),
            MembershipDate = r.GetFieldValue<DateOnly>(4).ToDateTime(TimeOnly.MinValue),
            Status = r.GetString(5),
            ActiveLoans = r.IsDBNull(6) ? 0 : (int)r.GetInt64(6)
        };

        private static Loan MapLoan(NpgsqlDataReader r) => new Loan
        {
            Id = r.GetInt32(0),
            BookId = r.GetInt32(1),
            BookTitle = r.GetString(2),
            MemberId = r.GetInt32(3),
            MemberName = r.GetString(4),
            IssueDate = r.GetFieldValue<DateOnly>(5).ToDateTime(TimeOnly.MinValue),
            DueDate = r.GetFieldValue<DateOnly>(6).ToDateTime(TimeOnly.MinValue),
            ReturnDate = r.IsDBNull(7) ? null : r.GetFieldValue<DateOnly>(7).ToDateTime(TimeOnly.MinValue),
            Status = r.GetString(8)
        };
    }
}
