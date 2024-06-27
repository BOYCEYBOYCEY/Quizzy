# Quizzy

**A User-Friendly Quiz Application**

## Overview

Quizzy is a C#/.NET Core MVC application with an Entity Framework Core data layer and a MySQL database that facilitates user knowledge assessment across various topics and difficulty levels. It offers user and admin login functionality, allowing users to track their quiz history and admins to manage quiz content.

## Features

* **User Accounts**
    * Users can register and log in to access quizzes.
    * Each user has a profile that stores past quiz records.
* **Quiz Management**
    * Admins can create, edit, and delete quizzes.
    * Quizzes can be categorized by topic and difficulty level.
    * Admins have control over the number of questions per quiz.
* **Question Types**
    * Quizzy currently supports various question types (consider listing specific types, e.g., multiple choice, true/false, fill-in-the-blank).
    * More question types can be added for future versions.
* **Quiz Taking**
    * Users can browse available quizzes based on topic and difficulty.
    * Quizzes can be time-bound or untimed.
    * Users receive immediate feedback upon completing a quiz.
    * Users can view their past quiz results, including scores and individual question details.
* **Data Persistence**
    * User accounts, quizzes, questions, answers, and quiz records are stored securely in a MySQL database using Entity Framework Core.

## Technology Stack

* Programming Language: C#
* Framework: ASP.NET Core MVC
* Data Access Layer: Entity Framework Core
* Database: MySQL

## Getting Started

**Prerequisites:**

1. A running MySQL database server.
2. Visual Studio or a similar C# development environment.
3. Necessary NuGet packages for C#, .NET Core, Entity Framework Core, and MySQL (refer to project documentation for details).

**Database Configuration:**

1. Update the `appsettings.json` file with your MySQL connection string details.
2. Ensure the database schema is created correctly using Entity Framework Core migrations.

**Run the Application:**

1. Open the project solution in your development environment.
2. Set the project as the startup project.
3. Run the application (typically by pressing F5).

## Usage

**User:**

* Register or log in to access quizzes.
* Browse available quizzes by topic and difficulty.
* Start a quiz, answer questions, and receive feedback.
* View past quiz results and individual question details.

**Admin:**

* Create, edit, and delete quizzes.
* Manage topics and difficulty levels.
* Control the number of questions per quiz.

## Further Development

* Implement additional question types.
* Enhance user interface and experience.
* Integrate with social media platforms for sharing quiz results.
* Create a mobile app version of Quizzy.

**Note:** This README provides a general overview. Specific configuration and usage instructions might be documented within the project itself (e.g., additional sections in the README or separate documentation files).

