using Microsoft.AspNet.SignalR;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using TodoList.Models;

namespace TodoList.Hubs
{
    public class Tasks : Hub
    {
        /// <summary>
        /// Our method to create a task
        /// </summary>
        public bool Add(Task newTask)
        {
            try
            {
                using (var context = new TodoListContext())
                {
                    var task = context.Tasks.Create();
                    task.title = newTask.title;
                    task.description = newTask.description;
                    task.status = newTask.status;
                    task.createdBy = newTask.createdBy;
                    task.assignedTo = newTask.assignedTo;
                    task.lastUpdated = DateTime.Now;
                    context.Tasks.Add(task);
                    context.SaveChanges();
                    Clients.All.taskAdded(task);
                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                Clients.Caller.reportError("Unable to create task. Make sure title length is between 10 and 140");
                return false;
            }
            catch (Exception ex)
            {
                Clients.Caller.reportError("Unable to create task. Make sure title length is between 10 and 140");
                return false;
            }

        }

        /// <summary>
        /// Update a task using
        /// </summary>
        public bool Update(Task updatedTask)
        {
            using (var context = new TodoListContext())
            {
                var oldTask = context.Tasks.FirstOrDefault(t => t.taskId == updatedTask.taskId);
                try
                {
                    if (oldTask == null)
                        return false;
                    else
                    {
                        oldTask.title = updatedTask.title;
                        oldTask.description = updatedTask.description;
                        oldTask.status = updatedTask.status;
                        oldTask.createdBy = updatedTask.createdBy;
                        oldTask.assignedTo = updatedTask.assignedTo;
                        oldTask.lastUpdated = DateTime.Now;
                        context.SaveChanges();
                        Clients.All.taskUpdated(oldTask);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Clients.Caller.reportError("Unable to update task. Make sure title length is between 10 and 140");
                    return false;
                }
            }

        }


        /// <summary>
        /// Delete the task
        /// </summary>
        public bool Remove(int taskId)
        {
            try
            {
                using (var context = new TodoListContext())
                {
                    var task = context.Tasks.FirstOrDefault(t => t.taskId == taskId);
                    context.Tasks.Remove(task);
                    context.SaveChanges();
                    Clients.All.taskRemoved(task.taskId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.reportError("Error : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To get all the tasks up on init
        /// </summary>
        public void GetAll()
        {
            using (var context = new TodoListContext())
            {
                var res = context.Tasks.ToArray();
                Clients.Caller.taskAll(res);
            }

        }
    }
}