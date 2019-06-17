# Library
#### By **Tessa Sullivan & Zach Weintraub**

## Description

This application (once complete) will allow a library employee to manage books and allow them to see what is checked out and what is available.

## Specifications
| Spec | Input | Output |
| :-------------     | :------------- | :------------- |
| Staff can add a book | Fills out form with title, author(s), and publication year|Book is added to database|
| Staff can edit a book's information | Makes changes to title, author(s), or publication year | Changes are saved in the database |
| Staff is able to enter information on how many copies the library has | Edits a book's information and changes stock information | Changes are saved in the database|
| Staff is able to checkout books | Selects book, enters patron information, and checks out book | Book is marked as checked out and number available goes down by 1|
| Staff is able to check in books | Selects book and checks in book | Book is marked as checked in and number available goes up by 1|
| Staff is able to search books by author | Enters search string | Matching books are displayed |
| Staff is able to search books by title | Enters search string | Matching books are displayed |
| Staff is able to display checked out books | Selects 'Checked out' from Library Staff pages | Books which are checked out are displayed, sorted by due date |
| Staff is able to add a patron | Enters patron information and saves | Patron is added to database |
| Staff is able to edit patron information | Edits patron information and saves | Changes are saved in the database|
| Staff is able to search for a patron | Enters search string | Matching patrons are listed |
| Staff is able to list books checked out for specific patron | Searches for patron and selects the "checked out" button | The books checked out by that patron are displayed |

## Setup/Installation Requirements
1. Clone this repository.
2. Install .Net 2.2 
    * Go to https://dotnet.microsoft.com/download/dotnet-core/2.2 and download the appropriate installer for your OS.
3. cd to Library and run ```dotnet restore```.
4. cd to Library.Tests and run ```dotnet restore``` (optional).  Numerous unit tests are included.
5. Install and configure MySQL - MAMP is recommended.
6. Run MySQL in the terminal with user root.  If no special password has been added to root, the command is ```mysql -uroot -proot```
7. Create the database and their tables.  You can either  

  a. Import the library.sql and library_test.sql files located in the repository's main directory by running mysql as root and running ```source library.sql``` and ```source library_test.sql```   
  
  b. Import the files through phpMyAdmin. 
8. In the terminal,run: ```dotnet run --project Library```.
9. Load http://localhost:5000 in your web browser.

## Known Issues
At this point in time, the majority of the code is for the backend of the application.  Therefore, the front end is barely there.

## Technologies Used

* C#
* HTML5 / CSS

## Support and contact details

_Contact Tessa Sullivan @ tessa.sullivan@gmail.com_

### License

*{This software is licensed under the MIT license}*

Copyright (c) 2019 **_Tessa Sullivan_**