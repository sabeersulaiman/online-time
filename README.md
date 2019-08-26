# Online Time Track

The OnlineTimeTrack is a web project created in Asp.net core Web-Api framework as Back End, C# code is using in Web Api. It is a individual project of me during my internship period @Fantacode Solutions. This project about tracking time of employees in a company. Now it have one role called User. In This project there are 3 modules mainly. They are:

- User
- Projects
- WorkLogs
- TimeLog

## User
User module will have the following features
- AddUser (SuperAdmin)
- Login
- ListUsers (SuperAdmin)
- GetUser
- UpdateUser (SuperAdmin) - optional

## Projects
Projects module are used to track the projects of the organization. It supports the following features.
- Add Projects (SuperAdmin)
- Update Project (SuperAdmin)
- Add Users to Project (SuperAdmin)
- List Projects

## WorkLogs
Worklogs can be created for each projects and users can record their estimated time for a work item and the actual time.
- Add to Worklog
- Update the worklog
- List WorkLog of User (Self, SuperAdmin)
- Delete

## TimeLogs
TimeLogs can be created against each worklog, and each worklog can have multiple timelog.
- Add timelog to a worklog
- list timelogs of a worklog
- Delete timelog

## Reporting
Reporting module is planned for future.