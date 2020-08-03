# Bookland

Bookland is a proof-of concept web application selling a wide variety of books to book lovers and book stores.

## Functionalities

### With this application, ordinary users (with no account) can:

    • View list of books and their details

### Registered individual users can:

    • Register and login with their accounts, and logout

    • Edit user profile

    • Purchase books and receive discount based on the purchase quantity

    • View their order history
    
### Registered company users can:

    • Purchase books and receive extra discounts if the company is authorised

### Users with admin role can:

    • Perform CRUD (Create, Read, Update, Delete) operations on book categories and cover types

    • Perform CRUD operations on book
    
    • Perform CRUD operations on other users and company list
    
    • Lock and unlock other users

    • To use the admin function, you can log into the admin account using this credentials (for testing purpose):

        Email: yufei.z222+admin@gmail.com
        Password: Admin123*

## Build With ASP.NET CORE MVC

    • Entity Framework Core
    
    • Microsoft SQL Server
    
    • Razor Pages
    
    • DataTables (Table plug-in for jQuery)


## Principles of code style

    Code style:

    • Write meaningful variable names;

    • Write meaningful comments;

    • Make consistent indentation and space for the code;

    • Write consistent naming convention, meaning we use pascal case for naming property, methods and class names and
      camel case for variables and methods' parameters;

    • Properly use nested loops and do not wirte long nested loops;

    • Do not repeat the same logic to avoid duplicated code. Follow KISS priciple: keep it simple and stupid;

    • Refactor the code after it works and try to make it clean to realise the same fucntion so that the code is more readable;

    • Do not write long methods, one method for one purpose;

    • Write good unit testing and it covers all the core functions;

    Design:

    • Design a clear structure for the project,

    • Seperate layers at least for business, data and presentation with three or more layers but less than five;

    • Consider the resusability of code;

    • Make good database design with necessary attributes and data type. Eliminate redundant data.

    • Set up proper authentication and athorisation for the security of the project, which inceases its reliability;

    • Consider modularity by dividing modules that serve different purposes to deal with the grow of the project.

## Contributors

• Yufei Zhang (Ivan Zhang)
