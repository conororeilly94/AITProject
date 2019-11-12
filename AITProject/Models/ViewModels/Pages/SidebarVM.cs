using AITProject.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AITProject.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }

        public SidebarVM(SidebarDTO row)
        {
            Id = row.Id;
            body = row.Body;
        }

        public int Id { get; set; }
        [AllowHtml]
        public string body { get; set; }
    }
}