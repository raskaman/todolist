using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using TodoList.Models;

namespace TodoList.Hubs
{
    public class Users : Hub
    {
        /// <summary>
        /// Our method to create a User
        /// </summary>
        public bool Add(User newUser)
        {
            try
            {
                using (var context = new TodoListContext())
                {
                    var user = context.Users.Create();
                    user.firstName = newUser.firstName;
                    user.lastName = newUser.lastName;
                    user.active = newUser.active;
                    context.Users.Add(user);
                    context.SaveChanges();
                    Clients.All.UserAdded(user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.reportError("Unable to create User: " + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Update a User using
        /// </summary>
        public bool Update(User updatedUser)
        {
            using (var context = new TodoListContext())
            {
                var oldUser = context.Users.FirstOrDefault(t => t.userId == updatedUser.userId);
                try
                {
                    if (oldUser == null)
                        return false;
                    else
                    {
                        oldUser.firstName = updatedUser.firstName;
                        oldUser.lastName = updatedUser.lastName;
                        oldUser.active = updatedUser.active;
                        
                        context.SaveChanges();
                        Clients.All.UserUpdated(oldUser);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Clients.Caller.reportError("Unable to update User: " + ex.Message);
                    return false;
                }
            }

        }
        
        /// <summary>
        /// Delete the User
        /// </summary>
        public bool Remove(int userId)
        {
            try
            {
                using (var context = new TodoListContext())
                {
                    var user = context.Users.FirstOrDefault(t => t.userId == userId);
                    context.Users.Remove(user);
                    context.SaveChanges();
                    Clients.All.UserRemoved(user.userId);
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
        /// To get all the Users up on init
        /// </summary>
        public void GetAll()
        {
            using (var context = new TodoListContext())
            {
                var res = context.Users.ToArray();
                Clients.Caller.UserAll(res);
            }

        }
    }
}