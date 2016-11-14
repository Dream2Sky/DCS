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
    public class AdminController
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
            List<RolesModel> roleList = GetRolesList().Where(n => n.Code != 0).ToList();
            List<Member> UserList = new List<Member>();

            foreach (var item in roleList)
            {
                UserList.AddRange(_memberBLL.GetUsersByRole(item.Code));
            }

            // 所有用户列表  包括 收集员 和 组长
            ViewData["UserList"] = GetMemberModel(UserList);

            //// 所有组长列表
            //ViewData["TeamLeaderList"] = UserList.Where(n => n.Role == 1).ToList();

            // 获取所有的角色列表
            ViewData["RoleList"] = GetRolesList();
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(MemberModel model)
        {
            AjaxResult ar = new Global.AjaxResult();
            if (model == null)
            {
                ar.state = ResultType.error;
                ar.message = "提交的数据不能为空";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            Member member = new Member();
            var state = _memberBLL.AddMember(model.Name, model.RoleCode, model.ParentName, ref member);

            if (state == OperatorState.repeat)
            {
                ar.state = ResultType.error;
                ar.message = "该用户名已存在，添加失败";
            }
            else if (state == OperatorState.error)
            {
                ar.state = ResultType.error;
                ar.message = "操作数据库发生错误，操作失败";
            }
            else if (state == OperatorState.success)
            {
                LogHelper.writeLog_info("添加新用户成功");
                ar.state = ResultType.success;
                ar.message = "添加新用户成功";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private List<RolesModel> GetRolesList()
        {
            return RolesManager.GetRolesList();
        }

        /// <summary>
        /// 将 List<Member> memberList 转化为 List<MemberModel> modelList
        /// </summary>
        /// <param name="memberList"></param>
        /// <returns></returns>
        [NonAction]
        private List<MemberModel> GetMemberModel(List<Member> memberList)
        {
            List<MemberModel> modelList = new List<Models.MemberModel>();
            foreach (var item in memberList)
            {
                MemberModel mm = new Models.MemberModel();
                mm.Name = item.Account;
                mm.ParentName = memberList.Where(n => n.Id == item.ParentId).SingleOrDefault().Account;
                mm.RoleCode = item.Role;

                modelList.Add(mm);
            }

            return modelList;
        }

        /// <summary>
        /// 加载组长列表 返回的是组长的账号列表
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTeamLeader(int role)
        {
            List<Member> memberList = _memberBLL.GetUsersByRole(role).ToList();
            List<string> teamLeader = new List<string>();
            foreach (var item in memberList)
            {
                teamLeader.Add(item.Account);
            }

            AjaxResult ar = new Global.AjaxResult();

            if (teamLeader == null || teamLeader.Count == 0)
            {
                ar.state = ResultType.error;
                ar.message = "加载组长列表失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ar.state = ResultType.success;
                ar.message = "加载组长列表成功";
                ar.data = teamLeader.ToJson();

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetRoles()
        {
            AjaxResult ar = new Global.AjaxResult();

            try
            {
                var json = GetRolesList().ToJson();

                ar.state = ResultType.success;
                ar.message = "获取角色列表成功";
                ar.data = json;

            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error;
                ar.message = "获取角色列表失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }
    }
}