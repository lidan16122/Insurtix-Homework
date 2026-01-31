using Insurtix_Server.Models.Constants;
using Insurtix_Server.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Insurtix_Server.BL.Services
{
    public class BooksService
    {
        private readonly BooksXML booksXML;
        public BooksService(BooksXML _booksXML)
        {
            booksXML = _booksXML;
        }
        public List<Book> GetAllBooks()
        {
            List<Book> books = booksXML.Doc.Root.Elements("book")
                .Select(x => new Book
                {
                    Category = (string)x.Attribute("category"),
                    Cover = (string)x.Attribute("cover"),
                    ISBN = (string)x.Element("isbn"),
                    Title = (string)x.Element("title"),
                    Lang = (string)x.Element("title")?.Attribute("lang"),
                    Authors = x.Elements("author").Select(a => (string)a).ToList(),
                    Year = (int)x.Element("year"),
                    Price = (double)x.Element("price")
                })
                .ToList();

            return books;
        }
    }
}
