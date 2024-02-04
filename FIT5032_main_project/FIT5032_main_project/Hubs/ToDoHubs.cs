using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class ToDoHub : Hub
    {
        private static List<(string Name, string Message)> todos = new List<(string, string)>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            var todoObjects = todos.Select(todo => new { Name = todo.Name, Message = todo.Message }).ToList();

            Clients.Caller.loadExistingTodos(todoObjects);
            return base.OnConnected();
        }


        public void Send(string name, string message)
        {
        
            todos.Add((name, message));

        
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}
