using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class ToDoHub : Hub
    {
        // Static list to store messages. Not suitable for production.
        private static List<(string Name, string Message)> todos = new List<(string, string)>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            // Convert the tuples to anonymous objects for serialization
            var todoObjects = todos.Select(todo => new { Name = todo.Name, Message = todo.Message }).ToList();

            // Send existing to-dos when a new client connects
            Clients.Caller.loadExistingTodos(todoObjects);
            return base.OnConnected();
        }


        public void Send(string name, string message)
        {
            // Add to static list
            todos.Add((name, message));

            // Send to all clients
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}
