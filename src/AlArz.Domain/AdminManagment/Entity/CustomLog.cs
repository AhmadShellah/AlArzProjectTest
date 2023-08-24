using Domain.Entity;
using System;

namespace AlArz.AdminManagement.Entity
{
    public class CustomLog : BaseEntity
    {
        public string Response { get; set; } = string.Empty;
        public string Request { get; set; } = string.Empty;
        public string ERPReference { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Result { get; set; } = true;
        public DateTime TransDate { get; set; } = DateTime.Now;
    }
}
