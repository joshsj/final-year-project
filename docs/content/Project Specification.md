# Project Specification

**Student:** Joshua Sexton-Jones, 28022626

**Date:** 21/10/2021

**Supervisor:** Nnamdi Anyameluhor

**Degree Course:** BEng (Hons) Software Engineering

**Project Title:** RendezVous

## Elaboration

RendezVous is a service for new and existing businesses to
verify employee attendance at 'job sites', i.e., a specific
location. Upon reaching and leaving a job site, an employee
provides verification information to 'check-in' and
'check-out' respectively.

The service uses GPS data to verify an employee's location,
alongside a identity verification process to ensure check-in
is valid, i.e., ensuring the end-user cannot be another
employee/individual.

Using the platform on the job should be as easy as possible,
so the system will incorporate handy features to ensure its
convenience. For example, providing notifications to users
when entering/leaving job sites.

RendezVous also offers integration with client systems to
register employees and forward check-in data, enabling
automated payroll for example.

## Project Aims

- Allow on-location workers to:
  - View their job sites
  - Check their personal details
  - Check-in and check-out of locations
  - Easily provide verification information at a job site
- Allow administrators to:
  - Configure company job sites
  - Handle verification issues
  - Manage employee details
  - Assign job sites to employees
- Develop a reliable method to validate a user's location
  and identity
- Extend the application's ease-of-use with small,
  additional features based on user research
- Implement method(s) for customers to integrate the service
  with their own systems

## Project Deliverables

The project will be developed as three systems: a RESTful
API serving the backend; a website for configuration and
management; and a cross-platform mobile application for
checking in/out on location.

Developing the RESTful API will be achieved using .NET Core,
as it was the main technology with which I worked on
placement. Alongside its high performance, it has: native
support for authentication, dependency injection, and ORMs
such as Entity Framework; highly-capable development tools
like Visual Studio and Rider, both available for students;
and almost any design pattern/architecture can be chosen
using either C# or F#.

The site will be developed using the React ecosystem, as it
offers a very wide additional frameworks/libraries to handle
all the fundamental aspects of the site, e.g., Redux for
state management, React-Bootstrap for UI development, and
auth0-react for Auth0 authentication. Due to my extensive
use of Vue on placement, the learning curve should be
shallow thanks to their fundamental similarities.

The check-in/out feature is separated into a dedicated
mobile application to provide as many possible sources for
identification as possible, e.g., NFC, biometrics, unique
device IDs, etc. Non-native mobile applications, such as
PWAs, do not have the capability to access this information
for security risks. The feature also lends itself to mobile
usage, as a phone is the most likely device an employee will
have at any job location.

Currently the two frontrunners to develop cross-platform
mobile applications are React Native and Xamarin, which are
both closely related to the frontend and backend
technologies respectively. I have chosen React Native as I
have no experience developing a UI with Xamarin and I do not
see the codebase lending utilities/business logic from the
backend system.

Since .NET Core is cross-platform, it can be deployed on any
OS along with the website.

I will follow an agile approach, managed using a Kanban
board. After creating user stories, tasks for development
and testing will be deduced using estimates in-line with the
deadlines on the action plan below. Lower-level agile
techniques, such as sprints, do not seem appropriate to the
development of this project; scheduling at university is
flexible and changeable, meaning the working hours for a
task may not be predictable even when constrained within an
estimate.

Note: I also chose React over Vue as it's currently
transitioning from v2 to v3 and some major libraries/tools
do not yet support the new major version.

## Action Plan

| Feature               | Platforms`*` | Objective                                                                                                            | Commencing <br> (Weeks)      |
| --------------------- | :----------: | -------------------------------------------------------------------------------------------------------------------- | :--------------------------- |
| Identity Verification |    Mobile    | Research mobile hardware (plus APIs) usable for verification <br> Design the verification process <br> Develop, Test | 25 Oct <br> [14, 15, 16]     |
| Location Verification |     Web      | Research and design location storage structure, Develop, Test                                                        | 15 Nov <br> [17, 18]         |
| Job Sites             |     Web      | Design management UI <br> Design storage structure <br> Develop, Test                                                | 29 Nov <br> [19, 20]         |
| Users                 | Web, Mobile  | Research authentication methods <br> Design roles and permissions <br> Develop, Test                                 | 13 Dec <br> [21, 24, 25, 26] |
| Customer Integration  |              | Research approaches to integrate with possible customer payroll systems                                              | 24 Jan <br> [27]             |
| Check-in Data         |     Web      | Design integrated and manual process, Develop, Test                                                                  | 31 Jan <br> [28, 29, 30]     |
| User Registration     |     Web      | Design integrated and manual processes, Develop, Test                                                                | 21 Feb <br> [31, 32]         |
| Extensions`**`        | Web, Mobile  | Attain user feedback on potential features                                                                           | 07 Mar <br> [33]             |
|                       |              | Design, develop, and test any additional feature(s)                                                                  | 14 Mar <br> [34, 35, 36, 37] |

**`*`** All objectives include API work

<!-- prettier-ignore-start -->
<!-- Prettier bug misformats the asterisks -->
**`**`** Time also usable to complete previous objective(s)
<!-- prettier-ignore-end -->

## Deadlines

| Submission                                             | Commencing | Week | Deadline     |
| ------------------------------------------------------ | ---------- | ---- | ------------ |
| Submit Project Specification and Ethics documents      | 18 Oct     | 13   | 21 Oct       |
| Provisional Contents Page                              | 14 Feb     | 30   | 18 Feb       |
| Draft Critical Evaluation <br>Draft Sections of Report | 14 Mar     | 34   | 18 Mar       |
| Project Deliverable <br> Project Report                | 04 Apr     | 37   | **07 April** |
