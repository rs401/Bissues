<!-- Thanks https://github.com/othneildrew/Best-README-Template -->
<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![actions-build-badge]][actions-build-url]
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/rs401/Bissues">
    <img src="img/logo2.svg" alt="Logo">
  </a>

  <h3 align="center">Bissues</h3>

  <p align="center">
    A Bug and Issue tracker project.
    <br />
    <a href="https://github.com/rs401/Bissues"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/rs401/Bissues">View Demo</a>
    ·
    <a href="https://github.com/rs401/Bissues/issues">Report Bug</a>
    ·
    <a href="https://github.com/rs401/Bissues/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
        <li><a href="#docker">Docker</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>

---

<!-- ABOUT THE PROJECT -->
## About The Project

Bissues is a Bug and Issue tracker, developed for a course project at 
[&#64;Champlain College](https://twitter.com/ChamplainEdu).

Bissues is a Bug and Issue tracking system for software developers and the users 
of their software. The purpose is to provide an open line of communication 
between developers and users, to resolve issues and bugs in a structured 
iterative manner.

When a user is not getting expected results from a program, the user files an 
“Issue” and requests support. A “Bug” is an error in code that allows undesired 
behavior. The issue raised by a user may be a bug in the code or a 
user/environment error (requirements not met, dependency missing). 

An issue tracker allows communication between the developer and the user, to 
resolve the issue. A bug tracker identifies a bug, and allows communication 
between developers to resolve the bug. A bug/issue tracker would allow all the 
developers and users to offer feedback on any bug/issue.

Users and developers can create an account in the Bissues system. Developers are 
able to create new projects and bissues to be tracked. Users are able to create 
new bissues for existing projects.

Each bissue has a details view that shows all communications about the bissue 
and actions taken to resolve the bissue. The details view also shows all 
timestamps related to the bissue: date created, date last modified, and 
resolution date.



### Built With

Bissues was developed using ASP.NET Core 5.0 with an extended ASP.NET Identity 
for user accounts, an MVC design pattern. The backend language is C# and uses 
Entity Framework ORM with a PostgreSQL database. The frontend uses cshtml Razor 
pages with Bootstrap, CSS3, and javascript. 

The applications I used to develop this project are MS VS Code editor, ZSH 
shell, Git version control and Mozilla Firefox web browser.

---

<!-- GETTING STARTED -->
## Getting Started

To run a local copy of Bissues you will need to install the prerequisites and 
follow the Installation steps.

### Prerequisites

* [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
* [PostgreSQL database](https://www.postgresql.org/download/)

You can follow the .NET 5.0 SDK installation instructions 
[here](https://docs.microsoft.com/en-us/dotnet/core/install/).

For the PostgreSQL database, you can either follow the instructions from the 
download page or you can run a docker container from 
[here](https://hub.docker.com/_/postgres).

> You can use SQLite if you would like to, just follow the additional SQLite 
installation steps below.

### Installation

Once you have the .NET 5.0 SDK installed and access to the .NET CLI Tools, you 
will need to install the Entity Framework tools. In the terminal run 
`dotnet tool install --global dotnet-ef` to install the EF tools. 

Next you will need to set some environment variables:

```
export DB_CONNECTION_STRING="<Your DB Connection String>"
export SEND_GRID_USER=<Your SendGrid User>
export SEND_GRID_EMAIL=<Your SendGrid Email>
export SEND_GRID_KEY=<Your SendGrid API Key>
```

Then you can obtain a copy of the source code and navigate to the 
`BissuesProject/Bissues/` directory and execute `dotnet-ef database update`, and 
then `dotnet run`. `dotnet run` will `restore` and `build` the project and then 
run the Bissues application.

The first run will seed the database with roles: Admin, Developer and User, as 
well as a test user for each role. 

|UserName|Password|Role|
|---    |---      |--- |
|admin@admin.com|Admin@123|Admin|
|dev@dev.com|Admin@123|Developer|
|user@user.com|Admin@123|User|

Example:

```bash
dotnet tool install --global dotnet-ef
git clone git@github.com:rs401/Bissues.git
cd BissuesProject/Bissues/
# At this point if you are going to use SQLite, you will need to skip to the 
# 'SQLite Additional steps' below.
dotnet-ef database update
dotnet run
```

SQLite Additional steps:

1. Open the Bissues/Startup.cs file and uncomment the section in 
`ConfigureServices` that sets the database connection for the DbContext to 
SQLite and comment out the section below it that sets the PostgreSQL connection.

Before:

```
public void ConfigureServices(IServiceCollection services)
{
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    services.AddDbContext<ApplicationDbContext>(options =>
        // options.UseSqlite(
        //     Configuration.GetConnectionString("DefaultConnection")));
        options.UseNpgsql(connectionString));
    services.AddDatabaseDeveloperPageExceptionFilter();

    services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
    services.AddControllersWithViews();
}
```

After:

```
public void ConfigureServices(IServiceCollection services)
{
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlite(
              Configuration.GetConnectionString("DefaultConnection")));
        // options.UseNpgsql(connectionString));
    services.AddDatabaseDeveloperPageExceptionFilter();

    services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
    services.AddControllersWithViews();
}
```

2. Delete the Bissues/Data/Migrations directory.
3. Create a new migration with `dotnet-ef migrations add "Init"`.
4. Now you can continue with
```
dotnet-ef database update
dotnet run
```

### Docker

(Coming Soon)

---

<!-- USAGE EXAMPLES -->
## Usage

Once Bissues is up and running you can log in with any of the three seeded users
create during the first build. I would recommend logging into the admin account
and changing the password as this is the only account that can add any users to
any roles. So keep this one safe. Though if you forget the password, you can 
delete the account from the database and restart the application and it will be
recreated with the default password.

As an "Admin", you can add Projects and manage all Projects, Bissues, Messages 
and Users. You can add and remove users from Roles, edit any bissue or message
(think sensor language or delete hate and spam), you decide if a Bissue is an
Issue or a Bug (set a Bissues Label).

As a "Developer" you can respond to Bissues and be assigned to a Bug.

As a "User" you can create Bissues and respond to Bissues.

Register an account by clicking "Register" at the top right of any page, enter 
your details and submit the form. You will then need to check your email for a 
confirmation link, you will not be able to sign in until you confirm your 
account.

Login to your account, then you can post a new Bissue under any project. The 
developers and community will see your Bissue and try to help. 

---

<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/rs401/Bissues/issues) for a list of 
proposed features (and known issues).

Future plans I have for this project are to bundle the project in a Docker 
image. Configure the application to use other database storage solutions. Add 
social login functionality.

---

<!-- CONTRIBUTING -->
## Contributing

If you would like to contribute, I would love to see your ideas and learn from 
them. 

1. Fork this repository
2. Checkout a new branch
3. Push your changes
4. Submit pull request

There is a first-timers pull request guide [here](https://github.com/firstcontributions/first-contributions).

---

<!-- LICENSE -->
## License

Distributed under the Apache License 2.0. See `LICENSE` for more information.

---

<!-- CONTACT -->
## Contact

Rich Stadnick - @[Res401](https://twitter.com/Res401) - rich.stadnick&gt;AT&lt;gmail.com

Project Link: [https://github.com/rs401/Bissues](https://github.com/rs401/Bissues)

---

<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements





<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/rs401/Bissues.svg?style=for-the-badge
[contributors-url]: https://github.com/rs401/Bissues/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/rs401/Bissues.svg?style=for-the-badge
[forks-url]: https://github.com/rs401/Bissues/network/members
[stars-shield]: https://img.shields.io/github/stars/rs401/Bissues.svg?style=for-the-badge
[stars-url]: https://github.com/rs401/Bissues/stargazers
[issues-shield]: https://img.shields.io/github/issues/rs401/Bissues.svg?style=for-the-badge
[issues-url]: https://github.com/rs401/Bissues/issues
[license-shield]: https://img.shields.io/github/license/rs401/Bissues.svg?style=for-the-badge
[license-url]: https://github.com/rs401/Bissues/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/richard-stadnick-3b4ab53b
[product-screenshot]: images/screenshot.png
[actions-build-badge]: https://github.com/rs401/Bissues/actions/workflows/BissuesBuild.UnitTests.yml/badge.svg
[actions-build-url]: https://github.com/rs401/Bissues/actions/workflows/BissuesBuild.UnitTests.yml
