# Car Rental System

## 🔐 Login Page

The login page is used to authenticate users before accessing the system. Roles Manager, Secretary are validated against the database.

![Login Screen](images/LogIn.png)

Users must enter a valid username and password. On successful login, they are redirected to the appropriate dashboard based on their role.


## 🔐 Password Management

This screen handles all password-related operations in the system and is used in three scenarios:

1. **First-Time Login (New Employee)**  
   - The employee logs in with their ID as a temporary password.
   - They are required to create a secure password and set answers to security questions for future identity verification.

2. **Forgotten Password**  
   - The user is prompted to answer three predefined security questions.
   - If the answers are correct, they proceed to create a new password.

3. **Scheduled Password Reset (Every 6 Months)**  
   - Every user is required to update their password every 6 months.
   - The system verifies their identity using the saved answers to security questions before allowing the reset.

### 🖼️ Screenshots

**Step 1: Answering Security Questions**

The user must provide correct answers to continue to the password reset screen.

![Security Questions](images/Password-Change2.png)

---

**Step 2: Enter New Password**

After successful verification, the user is asked to enter and confirm a new password.

![Enter New Password](images/Password-Change1.png)



## 📊 Main Dashboard (Manager & Secretary View)

After login, users are taken to their role-based dashboard. Both managers and secretaries see the list of active rentals, clients, vehicles, and key details for daily operations.

This dashboard is the central hub of the system and allows both roles to:
- View all current rentals and associated clients
- Access client and vehicle information
- View and filter reminders and notifications (like license/insurance expiration dates)
- Perform vehicle return and rental operations

The difference between the manager and secretary interface is mainly in **access control** — the manager has more permissions for user management and system settings.

---

### 👨‍💼 Manager Dashboard View

![Manager View](images/manager.png)

---

### 👩‍💼 Secretary Dashboard View

![Secretary View](images/secretary.png)

