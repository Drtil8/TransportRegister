# TransitRegister

TransitRegister was designed and implemented as part of the **Advanced Information Systems (PIS)** course at **Brno University of Technology, Faculty of Information Technology (BUT FIT)**.

TransitRegister is a web-based system designed to manage driver records, traffic offenses and vehicle registrations. The system enables collaboration between municipal offices and law enforcement to maintain accurate records and enforce traffic law enforcement. It replaces outdated paper-based processes with a digital solution, enhances accessibility and automation.

---

## Features
- **Driver Management**: Register, update and maintain driver records, including license details
- **Vehicle Registry**: Maintain centralized database of vehicles, including ownership history
- **Traffic Offense Tracking**: Record and process traffic offenses, assign penalty points, enforce fines
- **Stolen Vehicle Reports**: Register stolen vehicles and update their status upon recovery
- **User Roles and Authentication**: Supports administrators, officials and officers roles with appropriate access controls.
- **Automated Expiry System**: Periodic removal of penalty points according to predefined regulations
- **Server-side Filtering**: Advanced data filtering and searching for efficient record management

---

## Architecture
- **Frontend**: React.js for an interactive and responsive UI
- **Backend**: ASP.NET Core for handling business logic and API services
- **Database**: SQL Server with ORM Entity Framework for efficient data storage and management

---

## Installation
### Prerequisites
Ensure you have the following installed:
- Visual Studio 2022
- Node.js
- SQL Server
- .NET 6 SDK

### Setup Steps
1. Clone the repository and open the solution in Visual Studio
2. Navigate to the server project and apply database migrations:
   ```sh
   dotnet ef database update
   ```
3. Seed the database with initial data:
   ```sh
   dotnet run seed
   ```
4. Start the backend server in Visual Studio
5. Navigate to the frontend directory and install dependencies:
   ```sh
   npm install
   ```
6. Run the frontend:
   ```sh
   npm start
   ```
7. Access the system via:
  - Backend API: `https://localhost:7201/swagger`
  - Frontend UI: `https://localhost:5173/`

---

## Usage
### User Roles
- **Administrator**: Manages system users and roles
- **Officials**: Registers vehicles, drivers and processes traffic offenses
- **Officer**: Search driver and vehicle information, reports traffic violations

### Main Use Cases
- **Registering Driver**: Officials input driver details, assign licenses and track offense
- **Managing Vehicles**: Officials add vehicles, assign ownership and update statuses
- **Processing Offenses**: Officers reports offenses, assign penalties for approval, then officials approve or deny the penalties
- **Tracking Stolen Vehicles**: Stolen vehicles are marked and can be searched by officers
- **Penalty Management**: Points are automatically deducted after a specific period if no further offenses occur

---

## Contributors
- **David Drtil** (Backend & Stolen Vehicles Management & Deployment on Azure)
- **Dominik Pop** (Backend & Offense Management)
- **Tomáš Bartů** (Database & Authentication)
- **Matej Koreň** (Database & User Management)
- **Adam Hos** (Google Maps Integration)

---

## Evaluation
Final score: **58/60**
- Deduction due to unclear design in some use cases, causing ambiguity in implementation

