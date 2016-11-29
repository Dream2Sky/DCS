using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.dcs.entity;
using com.dcs.common;
using com.dcs.ibll;
using System.Web.Mvc;
using com.dcs.web.Globals;
using com.dcs.web.Models;

namespace com.dcs.web.Controllers
{
    public class CompetentController : Controller
    {
        private IMemberBLL _memberBLL;
        private UnderlingManager _underlingManager;

        public CompetentController(IMemberBLL memberBLL, UnderlingManager underlingManager)
        {
            _memberBLL = memberBLL;
            _underlingManager = underlingManager;
        }

        public ActionResult Index()
        {
            List<MemberModel> mmList = new List<MemberModel>();
            List<Member> memberList = new List<Member>();

            var parent = LoginManager.GetCurrentUser().Account;

            try
            {
                // 獲取當前管理員下面主管   和 主管下員工
                memberList = _underlingManager.GetUnderlingList();
                mmList = ModelChangeManager.ChangeTOMemberModel(memberList);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
            }

            ViewData["UserList"] = mmList;
            return View();
        }



    }
}