# LibraryOfMarko

A web application built using the MVC pattern in .NET core 2.2. The views are made using Razor pages. The app keeps account of users, books and currently rented books (as well as renting history) in a SQL database. Frontend is mostly in html with *minimal* Bootstrap.

## User interface

The user is greeted by a totally not generic index page:
![](http://i.imgur.com/w43pEkB.png "Index page")

##### Books
On the Books page, up to 5 most rented books (all time) are shown. In the upper left corner we can access the Search function or add a book to the database.

![alt text](https://i.imgur.com/KwhqyIk.png "Books page")
###### Add book
When adding a book, the Title, Author and Number of copies fields are validated before trying to update database. Further on, the form includes the optional cover image upload. The image is given a unique filename and copied to /wwwroot/images folder, while the path to image is stored in the database. In case the cover is not specified, a generic cover image is shown, as seen in the picture above.

![alt text](https://i.imgur.com/jOxPHMc.png "Add book")

###### Users

Users can be accessed either by selecting the first letter of their last name, or by using a built-in search bar.

![alt text](https://i.imgur.com/YLYHQ5C.png "Add book")


##### Books and users

Upon selecting a book/user, thei details alongside with options to edit their info (except for the unique ID) is given. In case of books, a "Rent" options is available
Book details:

![alt text](https://i.imgur.com/iNfpLJk.png "Book details")


Book edit page:

![alt text](https://i.imgur.com/eewYR0V.png "Edit book")

### Dependencies

The database is accessed using [Dapper](https://dapper-tutorial.net/).

License
----

MIT

**Free Software, Hell Yeah!**
