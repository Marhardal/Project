# Project Tracking Management System

## Overview

The Project Tracking Management System is a web-based solution developed to manage and monitor projects from submission to completion. It provides project registration, workflow tracking, status monitoring, reporting, and user management functionalities in a centralized platform.

The system helps organizations improve project visibility, streamline approvals, track progress, and generate insights through dashboards and reports.

---

## Features

### Project Management

* Create and manage projects
* Categorize projects by type and sector
* Assign locations and proponents
* Store project descriptions and supporting information

### Status Tracking

* Track project progress through configurable statuses
* Maintain a complete history of project updates
* Timeline-based project activity visualization
* Status duration tracking

### User Management

* User registration and authentication
* OTP-based login verification
* JWT authentication
* Role-based access control
* Permission management

### Dashboard & Analytics

* Project statistics and summaries
* Project distribution by type
* Status breakdown reports
* District/location-based visualizations
* Interactive charts and graphs

### Reporting

* Project performance reports
* Status tracking reports
* Location-based reports
* Exportable reporting functionality

### Notifications

* Activity tracking
* User alerts and notifications
* Project update history

---

## Technology Stack

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* JWT Authentication

### Frontend

* Blazor WebAssembly
* Bootstrap 5
* MudBlazor
* ApexCharts

### Other Tools

* Humanizer
* SVG Maps
* REST APIs

---

## System Modules

### Projects

Responsible for project registration, editing, categorization, and lifecycle management.

### Trackings

Records project status updates and maintains a complete audit trail.

### Categories

Manages project categories such as:

* Agriculture
* Energy
* Environmental Management
* Infrastructure
* Others

### Users & Roles

Handles authentication, authorization, and user permissions.

### Reports

Generates analytical and operational reports for management.

### Dashboard

Provides real-time project statistics and visual insights.

---

## Future Enhancements

* Budget Management Module
* Document Management System
* Workflow Automation
* Email Notifications
* Approval Workflows
* Mobile Responsive Dashboard
* GIS Integration
* Advanced Role and Permission Engine

---

## Installation

### Prerequisites

* .NET 9 SDK (or your target version)
* SQL Server
* Visual Studio 2022

### Setup

1. Clone the repository

```bash
git clone https://github.com/your-username/project-tracking-management-system.git
```

2. Configure the connection string in `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "YourConnectionString"
}
```

3. Apply migrations

```bash
dotnet ef database update
```

4. Run the API

```bash
dotnet run
```

5. Run the Blazor application

```bash
dotnet run
```

---

## Objectives

* Improve project monitoring and accountability
* Centralize project information
* Provide real-time reporting and analytics
* Enhance transparency in project management
* Support data-driven decision making

---

## License

This project is licensed under the MIT License.
