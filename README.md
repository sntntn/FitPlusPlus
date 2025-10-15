# FitPlusPlus

## Overview

**FitPlusPlus** is a modern fitness application developed as part of the **Software Development 2** course. The goal of the app is to simplify gym management, training reservations, payments, communication between clients and trainers, and analytics. The application is built using a **microservices architecture**, enabling easier maintenance and scalability.

This project is an **extension of the previous FitPlusPlus application**, which was developed by a previous team of students. Our team is continuing the development by adding new microservices, improving the existing ones, and expanding the overall functionality of the system.

---

## Team Members

### Current Development Team (2024):

1. **Marković Vukašin** – Student ID: 1051/2024
2. **Milenković Stefan** – Student ID: 1076/2024;
3. **Mitreski Milan** – Student ID: 1073/2024
4. **Filipović Natalija** – Student ID: 1013/2024
5. **Čolić Anja** - Student ID: 1059/2024

### Previous Development Team (2023):

3. **Belaković Nikola** – Student ID: 1023/2023
1. **Stanojević Lazar** – Student ID: 1013/2023
2. **Todorović Vasilije** – Student ID: 1015/2023

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
   - Uses **WebSockets** to enable **real-time messaging between clients and trainers**, ensuring instant updates without manual refresh.
   - Each chat session is identified by a unique WebSocket connection based on the trainer and client IDs.
   - This ensures efficient **synchronization** of messages between both participants in the chat.
   - **Payment Integration (PayPal)**
      - Clients can purchase **monthly chat sessions**, with payments validated through the **PaymentService**.  
      - After successful payment, clients gain **30-day access** to a private chat channel with their trainer.
    - **Notifications Integration**  
      - Integrated with the **NotificationService** to provide both **in-app notifications** (visible in the notification panel on the frontend) and **email alerts** after successful payment or new chat session creation.  
      ### Real-Time Chat Performance Demo
      This **[video](https://youtu.be/-41OJeE9N1I)** demonstrates the real-time messaging capabilities of the **ChatService** using WebSockets.  
      [![Watch the Demo](https://img.youtube.com/vi/-41OJeE9N1I/0.jpg)](https://youtu.be/-41OJeE9N1I)  

2. **VideoTrainingService**
   - Provides a **library of high-quality instructional trainings**.
   - Designed for **individual training** at home or gym, covering beginner to advanced levels.
   - **Provided functionalities for trainers:**
	   - preview of added exercises
	   - preview of created trainings
	   - add/remove exercise and upload video
	   - create/delete training
   - **Provided functionalities for clients:**
	   - preview of existing trainings including traininer's information
	   - full review of purchased trainings including videos for each exercise.

3. **ReservationService**
   - Enables **booking of individual and group training sessions**.
   - Supports **real-time scheduling, cancellation, and availability tracking**.
   - Integrated with NotificationService for real-time and email notifications on reservation updates.

4. **NotificationService**
   - Sends **push and email notifications** to clients and trainers.
   - Handles real-time **push** and **email notifications** for clients and trainers.
   - Delivers updates related to **training reservations**, **chat session payments**.
   - Implements a centralized notification system that ensures consistent communication across all services.
   - Provides a **notification history view** within the frontend, allowing users to review both **read** and **unread** notifications.

5. **AnalyticsService**
   - Tracks **individual and group** training sessions and provides **insightful statistics** about them
   - Offers dedicated views for **clients and trainers**, including:
      - Number of attended sessions
      - Number of cancelled sessions
      - A practical **review summary** reflecting client and trainer satisfaction
      - Detailed **monthly statistics** on training activity, including income information for trainers
      - **Charts** visualizing collaboration between clients and trainers

6. **Gateway and Discovery Service**
   - A **centralized API gateway** that directs requests to the correct microservice.
   - Facilitates **automatic detection and scaling** of microservices.
  
7. **Nutrition Service**
   - Manages **nutrition goals, meal plans and calorie tracking** for clients and trainers.
   - Allows trainers to define meal plans and clients to calculate personal goals and track calorie intake.
   - **Trainer Capabilities**
      - Add food items (name + calories)
      - Create meal plans for each goal type (lose, gain and maintain)
      - Each trainer can have up to 3 plans (one per goal type)
      - View, update and delete existing plans
      - All plans are stored seperatly per trainer
    - **Client Capabilities**  
      - Input personal data (age, sex, height, weight, goal, activity level, intensity)
      - Automatically calculates: BMI and Target Calories (kcal)
      - Select a trainer and view their nutrition plan based on goal type.
      - Interactive Calorie Tracker: Client can enter food in grams and automatically calculates consumed and remaining calories.

      
   This **[video](https://youtu.be/nSJhnaC0eAc?si=-Dm2DyJzZ1tnmKfo)** demonstrates the capabilities of the **Nutrition Service**: https://youtu.be/nSJhnaC0eAc?si=-Dm2DyJzZ1tnmKfo
   ![Adobe Express - file](https://github.com/user-attachments/assets/0ffc42e1-89b1-4107-938d-277fea981f7a)
   
---

## Technologies

- **Frontend:** Vue.js
- **Backend:** ASP.NET Core (C#)
- **Database:** MongoDB, Microsoft SQL Server
- **Containerization:** Docker and Docker Compose
- **Event Bus:** RabbitMQ for microservices communication
- **Communication Protocols:** WebSockets (real-time chat), GRPC and REST API for microservices communication
- **Payment Integration:** PayPal for chat session payments and booking training

## Platform Compatibility

FitPlusPlus has been **developed and tested** on the following operating systems:
- **Windows**
- **macOS**
- **Linux**

This ensures that developers and users can run the application smoothly across different environments.

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
   docker-compose -f docker-compose.yml -f docker-compose.development.yml up -d --build
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

