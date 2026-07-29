# Library Management System

A Blazor Server web application for managing a library’s collection, members, loans, and reports.

## Overview

This project provides a simple dashboard-style interface for librarians to:

- View library inventory and status summaries
- Add, edit, and manage books
- Track members and memberships
- Manage loans and returns
- Review reporting views for library operations

## Tech Stack

- ASP.NET Core Blazor Server
- .NET 10
- PostgreSQL
- Npgsql

## Features

- Dashboard with overview cards and search/filtering
- Book catalog management
- Member management
- Loan tracking
- Seeded sample data for quick testing

## Prerequisites

- .NET 10 SDK
- PostgreSQL database access

## Getting Started

1. Restore dependencies:
   ```bash
   dotnet restore
   ```

2. Configure the database connection string in appsettings.json if needed.

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open the app in your browser at:
   ```text
   http://127.0.0.1:5100
   ```

## Database Notes

The application initializes its schema automatically on startup and seeds sample users, books, members, and loans if the tables are empty.

## Default Login Credentials

The application seeds sample users for testing:

- Username: admin
- Password: admin123

## Project Structure

- Pages/ - Razor pages and UI components
- Services/ - Database, auth, and library business logic
- Shared/ - Shared layouts and navigation
- wwwroot/ - Static assets and styling

## Notes

This project is designed as a practical library management demo and can be extended with authentication, reporting enhancements, and additional inventory workflows.
