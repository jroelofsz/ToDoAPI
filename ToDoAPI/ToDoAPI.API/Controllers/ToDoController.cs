using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.DATA.EF; //to connect in to EF layer
using ToDoAPI.API.Models; //acces to the DTO's
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {

        ToDoEntities db = new ToDoEntities();

        public IHttpActionResult GetToDos()
        {
            List<TodoViewModel> todos = db.TodoItems.Include("Category").Select(r => new TodoViewModel()
            {
                TodoId = r.TodoId,
                Action = r.Action,
                Done = r.Done,
                CategoryId = r.CategoryId
            }).ToList<TodoViewModel>();

            if (todos.Count == 0)
            {
                return NotFound();
            }

            return Ok(todos);
        }//end GetToDos()


        public IHttpActionResult GetToDo(int id)
        {
            TodoViewModel todo = db.TodoItems.Include("Category").Where(r => r.TodoId == id).Select(r => new TodoViewModel()
            {
                TodoId = r.TodoId,
                Action = r.Action,
                CategoryId = r.CategoryId,
                Done = r.Done
            }).FirstOrDefault();

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }//end GetToDo

        
        public IHttpActionResult PostToDo(TodoViewModel todo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");

            TodoItem newToDoItem = new TodoItem()
            {
                Action = todo.Action,
                Done = todo.Done,
                CategoryId = todo.CategoryId
            };

            db.TodoItems.Add(newToDoItem);
            db.SaveChanges();
            return Ok(newToDoItem);
        }//end PostToDo()



        public IHttpActionResult PutToDo(TodoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            TodoItem existingToDoItem = db.TodoItems.Where(r => r.TodoId == todo.TodoId).FirstOrDefault();

            if (existingToDoItem != null)
            {
                existingToDoItem.TodoId = todo.TodoId;
                existingToDoItem.Action = todo.Action;
                existingToDoItem.Done = todo.Done;
                existingToDoItem.CategoryId = todo.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutToDo()



        public IHttpActionResult DeleteToDo(int id)
        {
            TodoItem todo = db.TodoItems.Where(r => r.TodoId == id).FirstOrDefault();

            if (todo != null)
            {
                db.TodoItems.Remove(todo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);    
        }
    }
}