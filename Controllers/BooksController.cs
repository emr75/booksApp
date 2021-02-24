using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace booksApp.Controllers
{
    public class BooksController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IList<Models.Book> bookList = new List<Models.Book>();
            //ViewBag.Output = "Everythignb's connected";

            //load poeple.xml
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("book");

                foreach (XmlElement b in books)
                {
                    Models.Book book = new Models.Book();
                    book.Id = b.GetElementsByTagName("id")[0].InnerText;
                    //Attempt at ID
                    string oz = book.Id;
                    int theId = int.Parse(oz);
                    book.Id = theId;
                    book.Title = b.GetElementsByTagName("title")[0].InnerText;
                    book.FirstName = b.GetElementsByTagName("firstname")[0].InnerText;
                    book.MiddleName = b.GetElementsByTagName("middlename")[0].InnerText;
                    book.LastName = b.GetElementsByTagName("lastname")[0].InnerText;

                    bookList.Add(book);
                }
            }
            return View(bookList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var book = new Models.Book();
            return View(book);
        }
        [HttpPost]
        public IActionResult Create(Models.Book b)
        {
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                //if file exists, load it and create new book 
                doc.Load(path);

                //create a new book
                XmlElement book = _CreateBookElement(doc, b);

                //get the root element
                doc.DocumentElement.AppendChild(book);
            } 
            else
            {
                //file doesn't exist, create and create new book
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("people");

                //Create a new person
                XmlElement book = _CreateBookElement(doc, b);
                root.AppendChild(book);
                doc.AppendChild(root);
            }
            doc.Save(path);
            return View();
        }

        private XmlElement _CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            XmlElement book = doc.CreateElement("book");

            XmlNode id = doc.CreateElement("id");
            XmlNode title = doc.CreateElement("title");
            title.InnerText = newBook.Title;
            XmlNode author = doc.CreateElement("author");
            XmlNode firstname = doc.CreateElement("firstname");
            firstname.InnerText = newBook.FirstName;
            XmlNode middlename = doc.CreateElement("middlename");
            middlename.InnerText = newBook.MiddleName;
            XmlNode lastname = doc.CreateElement("lastname");
            lastname.InnerText = newBook.LastName;

            author.AppendChild(firstname);
            author.AppendChild(middlename);
            author.AppendChild(lastname);

            book.AppendChild(title);
            book.AppendChild(author);

            return book;
        }
    }
}
