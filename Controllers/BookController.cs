using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMvcCurd.Data;
using FreshMvcCurd.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using System.Data;

namespace FreshMvcCurd.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration) 
       {
            this._configuration = configuration;
        }

        // GET: Book
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewAll",sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }

        // GET: Book/AddorEdit/5
        public IActionResult AddorEdit(int? id)
        {
          BookViewModel bookViewModel = new BookViewModel();
            if(id>0)
            {
                bookViewModel = FetchBookByID(id);
            }
            return View(bookViewModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult AddorEdit(int id, [Bind("BookID,Title,Author,price")] BookViewModel bookViewModel)
        {
           

            if (ModelState.IsValid)
            {
              using(SqlConnection sqlConnection=new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand("BookAddorEdit",sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("BookID",bookViewModel.BookID);
                    command.Parameters.AddWithValue("Title", bookViewModel.Title);
                    command.Parameters.AddWithValue("Author", bookViewModel.Author);
                    command.Parameters.AddWithValue("Price", bookViewModel.price);
                    command.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {
           BookViewModel bookViewModel= FetchBookByID(id);


            return View(bookViewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("BookDeleteByID", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("BookID", id);
               
                command.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
            
        }
        [NonAction]
       public BookViewModel FetchBookByID(int? id)
        {
            BookViewModel bookViewModel= new BookViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();   
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("BookID", id);
                sqlDa.Fill(dtbl);
                if(dtbl.Rows.Count==1)
                {
                    bookViewModel.BookID = Convert.ToInt32( dtbl.Rows[0]["BookID"].ToString());
                    bookViewModel.Title = dtbl.Rows[0]["Title"].ToString();
                    bookViewModel.Author = dtbl.Rows[0]["Author"].ToString();
                    bookViewModel.price = Convert.ToInt32( dtbl.Rows[0]["Price"].ToString());
                }
                return bookViewModel;
            }
        }
    }
}
