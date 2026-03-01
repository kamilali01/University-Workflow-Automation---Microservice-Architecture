# University Workflow Automation – Microservice Architecture

## Overview

This project presents a microservice-based implementation of a university workflow automation system developed using **ASP.NET Core (.NET 8)**.

The system represents the distributed architectural evolution of a previously implemented monolithic baseline and enables comparative analysis of:

- Modularity
- Scalability
- Service isolation
- Workflow orchestration
- Architectural trade-offs in higher education systems

This prototype supports the research study:

> **“Designing a Microservice-Based Software Architecture for University Workflow Automation”**

---

## Architectural Objectives

The primary goals of this microservice implementation are:

- Decompose a monolithic system into bounded domain services
- Apply the **Database-per-Service** pattern
- Introduce REST-based inter-service communication
- Implement an **API Gateway** (YARP)
- Demonstrate workflow orchestration across services
- Enable comparative analysis with a monolithic baseline

---

## System Architecture

The system consists of five independently deployable components:

### 1. StudentService
Manages student records.

### 2. CourseService
Manages academic course data.

### 3. EnrollmentService
Responsible for workflow orchestration:
- Validates Student and Course via REST calls
- Manages enrollment lifecycle (Pending → Approved/Rejected)
- Records audit logs
- Triggers notifications on approval

### 4. NotificationService
Stores notification logs triggered by workflow events.

### 5. API Gateway (YARP)
Provides a unified public entry point and routes requests to appropriate services.

---

## Architectural Principles Applied

- Domain-based Service Decomposition
- Database-per-Service Pattern
- API Gateway Pattern
- REST-based Inter-Service Communication
- Fault Isolation
- Audit Logging for Workflow Traceability
- Independent Deployability

---

## Service Ports

| Component | URL |
|------------|---------------------------|
| Gateway | http://localhost:5000 |
| StudentService | http://localhost:5001 |
| CourseService | http://localhost:5002 |
| EnrollmentService | http://localhost:5003 |
| NotificationService | http://localhost:5004 |

---

## Gateway Routes

The API Gateway exposes unified endpoints:

| Public Endpoint | Routed To |
|----------------|-----------|
| `/api/students` | StudentService |
| `/api/courses` | CourseService |
| `/api/enrollments` | EnrollmentService |
| `/api/notifications` | NotificationService |
| `/api/AuditLogs` | EnrollmentService |

Example:

GET http://localhost:5000/api/students

POST http://localhost:5000/api/enrollments


---

## Workflow Scenario

### Enrollment Creation

1. Client submits enrollment request.
2. EnrollmentService validates:
   - Student existence via StudentService
   - Course existence via CourseService
3. Enrollment is created with status `Pending`.
4. Audit log entry is recorded.

### Enrollment Approval

1. Enrollment status updated to `Approved`.
2. Audit log entry recorded.
3. NotificationService is triggered.
4. Notification log entry is stored.

---

## Data Management

Each service maintains its own independent SQLite database:

- `student.db`
- `course.db`
- `enrollment.db`
- `notification.db`

This ensures loose coupling and service-level data isolation.

---

## Fault Isolation Demonstration

If `NotificationService` becomes unavailable:

- Enrollment status updates still succeed
- Core workflow remains operational
- Audit logs record notification failure

This demonstrates improved fault tolerance compared to monolithic architecture.

---

## Monolith vs Microservices Comparison

| Criteria | Monolith | Microservices |
|-----------|-----------|--------------|
| Deployment | Single unit | Independent services |
| Coupling | Shared code & DB | Service isolation |
| Data Management | Single database | Database-per-service |
| Change Impact | Global | Localized per service |
| Fault Isolation | Low | Higher |
| Scalability | Mostly vertical | Horizontal per service |
| Latency | Lower | Slightly increased (REST calls) |
| Operational Complexity | Lower | Higher |

---

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- YARP Reverse Proxy
- REST-based communication
