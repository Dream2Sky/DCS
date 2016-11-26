using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.dcs.ibll;
using com.dcs.web.Models;
using com.dcs.web.Globals;
using com.dcs.entity;
using com.dcs.common;

namespace com.dcs.web.Controllers
{
    public class AccountController : Controller
    {
        private IMemberBLL _memberBLL;
        private UnderlingManager _underlingManager;
        public AccountController(IMemberBLL memberBLL, UnderlingManager underlingManager)
        {
            _memberBLL = memberBLL;
            _underlingManager = underlingManager;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountModel model)
        {
            AjaxResult result = new AjaxResult();

            if (model == null)
            {
                result.state = ResultType.error.ToString();
                result.message = "提交的数据为空，登陆失败";

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            Member member = new Member();
            var state = _memberBLL.Login(model.Account, model.Password, ref member);

            if (state == LoginState.empty)
            {
                result.state = ResultType.error.ToString();
                result.message = "提交的数据为空，登陆失败";
            }
            else if (state == LoginState.account_error)
            {
                result.state = ResultType.error.ToString();
                result.message = "提交的账号不存在，登陆失败";
            }
            else if (state == LoginState.password_error)
            {
                result.state = ResultType.error.ToString();
                result.message = "密码错误，登陆失败";
            }
            else if (state == LoginState.failed)
            {
                result.state = ResultType.error.ToString();
                result.message = "系统错误，登陆失败";
            }
            else if (state == LoginState.success)
            {
                result.state = ResultType.success.ToString();
                result.message = "登陆成功";

                //获取每个角色的首页地址
                result.data = RolesManager.GetRoleInfo(member.Role).Url;

                // 保存登陆用户到 session
                LoginManager.SetCurrentUser(member);
                // 缓存下属账号名单 到 session
                _underlingManager.InitUnderlingList(member);

                LogHelper.writeLog_info("账号" + member.Account + "于" + DateTime.Now.ToString() + "登陆成功。");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.state = ResultType.error.ToString();
                result.message = "系统错误，登陆失败";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            LoginManager.Clean();
            return RedirectToAction("Login", "Account");
        }
    }
}