using com.dcs.common;
using com.dcs.entity;
using com.dcs.ibll;
using com.dcs.web.Globals;
using com.dcs.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.dcs.web.Controllers
{
    public class AdminController:Controller
    {
        private IMemberBLL _memberBLL;

        public AdminController(IMemberBLL memberBLL)
        {
            _memberBLL = memberBLL;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列出除了管理员自身之外的所有用户
        /// 
        /// 并且加载所有的角色类型 和 组长列表
        /// 角色类型  RoleList
        /// 
        /// 组长列表  TeamLeaderList
        /// 
        /// 所有的用户列表  UserList  
        /// 这里的用户列表包括收集员 和 组长
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberList()
        {
            return View();
        }

        /// <summary>
        /// 获取所有主管的列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCompetents()
        {
            AjaxResult ar = new Globals.AjaxResult();

            try
            {
                List<MemberModel> 
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }


        [HttpPost]
        public ActionResult GetRoles()
        {
            return View();
        }
    }
}