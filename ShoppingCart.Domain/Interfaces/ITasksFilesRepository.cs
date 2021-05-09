using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ITaskFileRepository
    {
        void AddFileToTask(TaskFile  f);
    }
}
