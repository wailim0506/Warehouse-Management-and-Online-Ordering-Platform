# Legend Motor Company Integrated System

![Version](https://img.shields.io/badge/version-1.0.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

## Table of Contents

- [Legend Motor Company Integrated System](#legend-motor-company-integrated-system)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Features](#features)
    - [Order Management](#order-management)
    - [Spare Part Management](#spare-part-management)
    - [Stock Management](#stock-management)
    - [User Management](#user-management)
    - [Invoice Management](#invoice-management)
  - [System Requirements](#system-requirements)
    - [Hardware Requirements](#hardware-requirements)
    - [Software Requirements](#software-requirements)
  - [Installation](#installation)
  - [Architecture](#architecture)
  - [Database Schema](#database-schema)
  - [Security](#security)
  - [License](#license)
  - [Acknowledgments](#acknowledgments)

## Overview

The Legend Motor Company Integrated System is a comprehensive software application designed to streamline the operations of Legend Motor Company. This centralized platform manages various business aspects including order processing, inventory control, customer management, and financial transactions.

The system includes modules for:
- Order Management
- Spare Part Management
- Stock Management
- User Management
- Invoice Management

## Features

### Order Management
- Process and track customer orders
- Manage order status (Pending, Processing, Ready to Ship, Shipped)
- Generate order serial numbers
- Associate orders with staff accounts for accountability
- Support for delivery relay options

### Spare Part Management
- Track part details including name, quantity, supplier, and category
- Monitor reorder and danger levels for inventory control
- Process reorder requests
- Track part modifications with staff accountability

### Stock Management
- Monitor on-sale quantities for both regular and LM customers
- Track product details including price, description, and category
- Enable/disable products as needed
- Track product modification history

### User Management
- Secure account management for both staff and customers
- Role-based permissions system
- Track login history for security auditing
- Support customer profiles including shipping/billing information

### Invoice Management
- Generate invoices linked to orders
- Track invoice status
- Process payments

## System Requirements

### Hardware Requirements
- Processor: 2.0 GHz or faster
- RAM: 4GB minimum (8GB recommended)
- Disk Space: 1GB minimum for application plus database storage

### Software Requirements
- Windows 10 or later
- .NET Framework 4.7.2 or later
- MySQL 5.7 or later / MariaDB 10.4 or later
- Google Maps Static API key (for location features)

## Installation

The setup process involves:

1. **Configuring Application Settings**
   - Set up database connection strings
   - Configure system parameters

2. **Obtaining a Google Maps Static API Key**
   - Register for a Google Cloud account
   - Enable the Google Maps Static API
   - Generate and configure your API key

3. **Database Setup (Docker - Active Path)**
   - Install Docker Desktop
   - Run the provided Docker Compose configuration:
     `docker compose -f docker/Docker-compose.yml up --build`
   - The database will be automatically set up with the required schema

4. **Running the Application**
   - Launch the WinForms application
   - Log in with the provided default credentials

For detailed installation instructions, refer to the [Docker Wiki](docker/wiki/README.md).

## Architecture

The system is built using a multi-tier architecture:

- **Presentation Layer**: WinForms application providing the user interface
- **Business Logic Layer**: .NET-based controllers handling business rules
- **Data Access Layer**: Database interaction components
- **Database**: MySQL/MariaDB relational database

## Database Schema

The system utilizes a comprehensive relational database with multiple interconnected tables:

- **Customer Management**: customer, customer_account, customer_login_history
- **Staff Management**: staff, staff_account, staff_login_history, department, jobtitle
- **Inventory Management**: spare_part, product, category, supplier
- **Order Processing**: order_, order_line, shipping_detail, deliveryrelay
- **Financial Records**: invoice, discount

## Security

The system implements multiple security measures:

- Password hashing using bcrypt
- Role-based access control
- Login history tracking
- Account status management


## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- ITP4915M Software Engineering course
- SE1D Group 4 development team
- Mock data providers and testers
