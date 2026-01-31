using Insurtix_Server.Models.Constants;
using Insurtix_Server.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public bool AddNewBook(Book book)
        {
            try
            {
                XElement newBook = new XElement("book",
                    new XAttribute("category", book.Category ?? ""),
                    book.Cover != null ? new XAttribute("cover", book.Cover) : null,
                    new XElement("isbn", book.ISBN),
                    new XElement("title", new XAttribute("lang", book.Lang ?? "en"), book.Title),
                    book.Authors.Select(a => new XElement("author", a)),
                    new XElement("year", book.Year),
                    new XElement("price", book.Price)
                );

                booksXML.Doc.Root.Add(newBook);

                booksXML.SaveDoc();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"error adding book: {e.Message}");
                return false;
            }
        }
    }
}
