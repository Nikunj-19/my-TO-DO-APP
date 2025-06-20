using Microsoft.AspNetCore.Mvc;
using TodoWebApp.Models;

namespace TodoWebApp.Controllers
{
    public class TodoController : Controller
    {
        private static List<TodoItem> _todoList = new();

        public IActionResult Index()
        {
            return View(_todoList);
        }

        [HttpPost]
        public IActionResult Add(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                _todoList.Add(new TodoItem { Id = _todoList.Count + 1, Title = title, IsCompleted = false });
            }
            return RedirectToAction("Index");
        }

        public IActionResult Toggle(int id)
        {
            var task = _todoList.FirstOrDefault(t => t.Id == id);
            if (task != null) task.IsCompleted = !task.IsCompleted;
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var task = _todoList.FirstOrDefault(t => t.Id == id);
            if (task != null) _todoList.Remove(task);
            return RedirectToAction("Index");
        }
    }
}
