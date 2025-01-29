# FitPlusPlus

## Overview

**FitPlusPlus** is a modern fitness application developed as part of the **Software Development 2** course. The goal of the app is to simplify gym management, training reservations, payments, communication between clients and trainers, and analytics. The application is built using a **microservices architecture**, enabling easier maintenance and scalability.

This project is an **extension of the previous FitPlusPlus application**, which was developed by a previous team of students. Our team is continuing the development by adding new microservices, improving the existing ones, and expanding the overall functionality of the system.

---

## Team Members

### Current Development Team (2024):

1. **Aleksandra Labović** – Student ID: 1025/2024&#x20;
2. **Vukašin Marković** – Student ID: 1051/2024
3. **Stefan Milenković** – Student ID: 1076/2024&#x20;
4. **Milan Mitreski** – Student ID: 1073/2024
5. **Natalija Filipović** – Student ID: 1013/2024

### Previous Development Team (2023):

1. **Lazar Stanojević** – Student ID: 1013/2023
2. **Vasilije Todorović** – Student ID: 1015/2023
3. **Nikola Belaković** – Student ID: 1023/2023

GitHub Repository of the Previous Project: [FitPlusPlus](https://github.com/lazars01/FitPlusPlus)

---

## Microservices

The FitPlusPlus application consists of multiple microservices, some developed by the previous team and others introduced in this extension.

### Existing Microservices (Developed by the Previous Team):

1. **IdentityServer**
   - Manages user authentication and authorization.

2. **ClientService**
   - Handles client registration, profile management, and membership data.

3. **TrainerService**
   - Manages trainer profiles, schedules, and training history.

4. **ReviewService**
   - Allows clients to leave reviews and ratings for trainers.

5. **PaymentService**
   - Processes payments for training sessions.

6. **Frontend**
   - A single-page application built with Vue.js for interacting with the backend services.

### New Microservices (Developed by the Current Team):

1. **ChatService**
   - Enables trainers to provide **online mentorship** to their clients via chat.
   - Clients can directly communicate with trainers for **training advice, progress tracking, and personalized plans**.
   - Paid mentorship sessions can be booked and managed within the app.

2. **VideoTrainingService**
   - Provides a **library of high-quality instructional videos** on correct exercise execution.
   - Designed for **independent training** at home, covering beginner to advanced levels.
   - Trainers can **upload and manage** their training materials.

3. **ReservationService**
   - Enables **booking of individual and group training sessions**.
   - Supports **real-time scheduling, cancellation, and availability tracking**.

4. **ManagerService**
   - A service for **gym administrators**, providing tools for managing trainers, clients, and finances.
   - Generates **performance reports and financial summaries**.

5. **NotificationService**
   - Sends **push and email notifications** to clients, trainers, and administrators.
   - Includes **reservation confirmations, training reminders, and membership renewal alerts**.

6. **AnalyticsService**
   - Creates **detailed statistics** on training sessions, client engagement, and trainer performance.
   - Provides insights into **popular training types and user activity trends**.

7. **Gateway and Discovery Service**
   - A **centralized API gateway** that directs requests to the correct microservice.
   - Facilitates **automatic detection and scaling** of microservices.

---

## Technologies

- **Frontend:** Vue.js
- **Backend:** ASP.NET Core (C#)
- **Database:** MongoDB, Microsoft SQL Server
- **Containerization:** Docker and Docker Compose
- **Event Bus:** RabbitMQ for microservices communication
- **Communication Protocols:** GRPC and REST API for microservices communication
- **Payment Integration:** PayPal for chat session payments

---

## Running the Application

To start the updated application:

1. Clone the repository:

   ```bash
   git clone <repository URL>
   cd FitPlusPlus
   ```

2. Start all microservices from FitPlusPlus/Fitness/Backend:

   ```bash
   docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build
   ```

3. Start the frontend service from FitPlusPlus/Fitness/Frontend directory:

   ```bash
   npm install
   npm run serve
   ```

4. Open the application in your browser:

   ```
   http://localhost:8080
   ```

