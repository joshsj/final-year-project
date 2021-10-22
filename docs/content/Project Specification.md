# Project Specification

**Student:** Joshua Sexton-Jones, 28022626

**Date:** 21/10/2021

**Supervisor:** Nnamdi Anyameluhor

**Degree Course:** BEng (Hons) Software Engineering

**Project Title:** RendezVous

## Elaboration

RendezVous is a service for new and existing businesses to verify employee
attendance at 'job sites', i.e., a specific location. Upon reaching and leaving
a job site, an employee provides verification information to 'check-in' and
'check-out' respectively.

The service uses GPS data to verify an employee's location, alongside an
automated identity verification process, e.g., facial recognition. The employee
provides a photograph, which is validated against their account's picture. In
the case of failure, manual verification is used as a fallback.

By the nature of working on-location, an internet connection is not a guarantee;
to ensure check-ins are not lost, they can be cached on the employee's device to
be submitted once a connection is restored. The check-in process is further
enhanced by notifying users upon entering/leaving job sites.

RendezVous also offers integration with client systems to forward check-in data,
enabling automated payroll for example.

## Project Aims

- Produce an application for employees to:
  - View their job sites
  - Check their personal details
  - Check-in and check-out of locations
  - Easily provide verification information at a job site
- Produce an application for employers to:
  - Configure company job sites
  - Handle manual verification
  - Manage employee details
  - Assign job sites to employees
- Develop a reliable method to validate a user's location and identity
- Extend the usability of the application by incorporating:
  - Notifications
  - Check-in caching
- Implement method(s) for customers to integrate the service with their own
  systems

## Project Deliverables

The system will be implemented using web technologies, serving a RESTful API as
the backend and a website developed as an SPA for the frontend.

The frontend will be developed using the React ecosystem, as it offers a very
wide additional frameworks/libraries to handle all the fundamental aspects of
the site, e.g., Redux for state management, React-Bootstrap for UI development,
and auth0-react for Auth0 authentication. Crucially, the ecosystem also has
immediate support for developing a PWA, using templates or an additional
library; this enables notification and offline storage access to cover all
aspects of the project description

Note: Vue was also considered due to my extensive use on placement. However, Vue
is currently transitioning from v2 to v3 and some major libraries do not yet
support the new major version. Combined with the support from my 'Applications:
Architectures and Frameworks' module, React was the better option.

The .NET Core framework will framework the RESTful API, as it was the main
technology with which I worked on placement. Alongside its high performance, it
has: native support for authentication, dependency injection, and ORMs such as
Entity Framework; highly-capable development tools like Visual Studio and Rider,
both available for students; and external tools to aid development with SPA
technologies, such as NSwag for API-client generation.

Since all technologies are cross-platform across the frontend and backend, the
system will be deployable on any operating system.

I will follow an agile approach, managed using a Kanban board. After creating
user stories, tasks for development and testing will be deduced using estimates
in-line with the deadlines on the action plan below. Lower-level agile
techniques, such as sprints, do not seem appropriate to the development of this
project; scheduling at university is flexible and changeable, meaning the
working hours for a task may not be predictable even when constrained within an
estimate.

## Action Plan

> This should be a table listing the jobs that need doing to succeed with your
> project. This is your list of objectives. Against each job you should put a
> date by when it needs to be done. You and your supervisor will use this to
> ensure that the project remains on schedule. You could also use a graphical
> technique to present the information. Include optional deadlines such as the
> information review, contents page, and draft critical evaluation submissions.

| Objective  | Task                                                    | End Date    |     |
| ---------- | ------------------------------------------------------- | ----------- | :-: |
| Submission | Information Review                                      | 03 December |  F  |
| Submission | Provisional Contents Page                               | 18 February |  F  |
| Submission | Draft Critical Evaluation <br> Sections of Draft Report | 18 March    |  F  |
| Submission | Project Deliverable & Report                            | 07 April    |  F  |
