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
    public class AdminController : Controller
    {
        private IMemberBLL _memberBLL;
        private ICustomItemBLL _customItemBLL;
        private IInformationBLL _informationBLL;
        public AdminController(IMemberBLL memberBLL, ICustomItemBLL customItemBLL, IInformationBLL informationBLL)
        {
            _memberBLL = memberBLL;
            _customItemBLL = customItemBLL;
            _informationBLL = informationBLL;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 页面加载时 需要调用到的方法

        /// <summary>
        /// 获取当前登陆管理员下的所有员工 包括主管 收集员 普通员工
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberList()
        {
            List<MemberModel> mmList = new List<MemberModel>();
            List<Member> memberList = new List<Member>();

            var parent = LoginManager.GetCurrentUser().Account;

            try
            {
                // 獲取當前管理員下面主管   和 主管下員工
                IEnumerable<Member> competentList = _memberBLL.GetUsersByParent(parent);
                foreach (var item in competentList)
                {
                    memberList.AddRange(_memberBLL.GetUsersByParent(item.Account).ToList());
                }
                memberList.AddRange(competentList.ToList());

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

        /// <summary>
        /// 获取所有主管的列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCompetents()
        {
            AjaxResult ar = new Globals.AjaxResult();
            List<string> competentList = new List<string>();

            try
            {
                var memberList = _memberBLL.GetUsersByRole(RolesManager.GetRolesCode("Competent")).Where(n => n.Parent == LoginManager.GetCurrentUser().Account);
                foreach (var item in memberList)
                {
                    competentList.Add(item.Account);
                }

                ar.state = ResultType.success;
                ar.data = competentList.ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error;
                ar.message = "获取主管列表失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色列表 当前角色下面有权限添加的角色列表
        /// 比如管理员 有权添加 主管、收集员和 普通员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRoles()
        {
            AjaxResult ar = new AjaxResult();
            try
            {
                var code = LoginManager.GetCurrentUser().Role;
                List<RolesModel> rmList = RolesManager.GetRolesList().Where(n => n.Code != code).ToList();

                ar.state = ResultType.success;
                ar.data = rmList.ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error;
                ar.message = "无法获取角色列表";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据用户的账号  获取 对应的 用户名
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMemberByAccount(string Account)
        {
            AjaxResult ar = new AjaxResult();
            try
            {
                var UserName = _memberBLL.GetUserByAccount(Account).Name;
                ar.state = ResultType.success.ToString();
                ar.data = UserName;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "获取用户名失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 用户操作


        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddMember(MemberModel model)
        {
            AjaxResult ar = new AjaxResult();

            if (model == null)
            {
                ar.state = ResultType.error;
                ar.message = "提交的数据为空，添加失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                Member member = new Member();

                var currentUser = LoginManager.GetCurrentUser();

                #region 判斷上級是否是同級了

                object parent = null;
                if (model.ParentName == null)
                {
                    parent = currentUser.Account;
                }
                else
                {
                    var parentResult = IsSameRange(model.ParentName, model.RoleCode);
                    if (parentResult == OperatorState.empty)
                    {
                        ar.state = ResultType.error.ToString();
                        ar.message = "不能獲取對應的上級賬號，添加新用戶失敗";

                        return Json(ar, JsonRequestBehavior.AllowGet);
                    }
                    else if (parentResult == OperatorState.error)
                    {
                        parent = currentUser.Account;
                    }
                    else if (parentResult == OperatorState.success)
                    {
                        parent = model.ParentName;
                    }
                }
                #endregion

                var result = _memberBLL.AddMember(model.Name, model.RoleCode, parent.ToString(), currentUser.CompanyCode, ref member);

                if (result == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "添加新用户失败";
                }
                else if (result == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                    ar.data = ModelChangeManager.ChangeTOMemberModel(member);
                    ar.message = "添加成功";
                }

                return Json(ar, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error;
                ar.message = "系统错误，添加新用户失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateMember(MemberModel model)
        {
            AjaxResult ar = new AjaxResult();
            if (model == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的數據爲空，修改用戶失敗";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var member = _memberBLL.GetUserByAccount(model.Account);
                if (member == null)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "不存在該賬號，修改用戶失敗";

                    return Json(ar, JsonRequestBehavior.AllowGet);
                }

                object parent = null;
                var currentUser = LoginManager.GetCurrentUser();
                OperatorState resultParent = IsSameRange(model.ParentName, model.RoleCode);
                if (resultParent == OperatorState.empty)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "不能獲取對應的上級賬號，修改用戶失敗";

                    return Json(ar, JsonRequestBehavior.AllowGet);
                }
                else if (resultParent == OperatorState.error)
                {
                    parent = currentUser.Account;
                }
                else if (resultParent == OperatorState.success)
                {
                    parent = model.ParentName;
                }

                var result = _memberBLL.UpdateMember(model.Account, model.Name, model.RoleCode, model.ParentName);

                if (result == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "修改該用戶失敗";
                }
                else if (result == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                    ar.message = "修改該用戶成功";
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "修改該用戶發生異常";

            }
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteMember(string Account)
        {
            AjaxResult ar = new Globals.AjaxResult();
            if (Account == string.Empty || Account == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的賬號爲空";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var member = _memberBLL.GetUserByAccount(Account);
                if (member == null)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "要刪除的賬戶不存在";

                    return Json(ar, JsonRequestBehavior.AllowGet);
                }

                OperatorState result = _memberBLL.DeleteMember(member);
                if (result == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "刪除賬號失敗";
                }
                else if (result == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                    ar.message = "刪除賬號成功";
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error;
                ar.message = "系統錯誤，刪除賬號失敗";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult ImportPage()
        {
            return View();
        }

        #region 自定义项操作

        /// <summary>
        /// 根据当前登陆用户的用户名获取其自定义项
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCustomItems()
        {
            AjaxResult ar = new AjaxResult();
            try
            {
                // 獲取當前登陸用戶
                var currentUser = LoginManager.GetCurrentUser();

                List<CustomItem> CustomItemList = new List<CustomItem>();
                List<CustomItemModel> CustomItemModelList = new List<CustomItemModel>();

                var state = _customItemBLL.GetCustomItems(currentUser.Account, ref CustomItemList);
                if (state == OperatorState.empty)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "不能获取当前用户所定义的自定义项";

                }
                else if (state == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "获取自定义项失败";
                }
                else if (state == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                    ar.data = ModelChangeManager.ChangeTOCustomItemModel(CustomItemList).ToJson();
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error;
                ar.message = "系统错误，获取自定义项失败";
            }
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加自定义项
        /// </summary>
        /// <param name="customItemName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCustomItems(string customItemName)
        {
            AjaxResult ar = new Globals.AjaxResult();
            if (customItemName == string.Empty)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的数据为空，添加自定义项失败";
                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var currentUser = LoginManager.GetCurrentUser();
                CustomItem ci = new CustomItem();
                var state = _customItemBLL.AddCustomItems(currentUser.Account, customItemName, ref ci);

                if (state == OperatorState.repeat)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "所提交自定义项已存在，添加自定义项失败";
                }
                else if (state == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "添加自定义项失败";
                }
                else if (state == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                    ar.data = ModelChangeManager.ChangeTOCustomItemModel(ci).ToJson();
                    ar.message = "添加自定义项成功";
                }
            }
            catch (Exception ex)
            {

                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，添加自定义项失败";


            }
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改自定义项
        /// </summary>
        /// <param name="customItemName"></param>
        /// <param name="customItemContent"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateCustomItems(string customItemName, string customItemContent)
        {
            AjaxResult ar = new Globals.AjaxResult();

            if (customItemName == string.Empty || customItemContent == string.Empty)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的数据为空, 修改自定义项失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                CustomItem customItem = new CustomItem();
                var state = _customItemBLL.UpdateCustomItems(customItemName, customItemContent);

                if (state == OperatorState.empty)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "不能找到相应的自定义项，修改自定义项失败";
                }
                else if (state == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "修改自定义项失败";
                }
                else if (state == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，修改自定义项失败";
            }
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除自定义项
        /// </summary>
        /// <param name="customItemName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCustomItems(string customItemName)
        {
            AjaxResult ar = new AjaxResult();

            if (customItemName == string.Empty)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的数据为空， 删除自定义项失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var state = _customItemBLL.DeleteCustomItems(customItemName);
                if (state == OperatorState.empty)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "不存在相应的自定义项, 删除自定义项失败";
                }
                else if (state == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "删除自定义项失败";
                }
                else if (state == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，删除自定义项失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 单条添加数据
        /// </summary>
        /// <param name="InformationModel"></param>
        /// <param name="CustomItemModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Import(Information InformationModel, List<FormModel> CustomItemModel)
        {
            AjaxResult ar = new AjaxResult();
            if (InformationModel == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交数据有误，添加新纪录失败";
                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var currentUser = LoginManager.GetCurrentUser();
                if (!_informationBLL.IsExsit(InformationModel.Phone,
                    InformationModel.QQ, InformationModel.WebCat, currentUser.CompanyCode))
                {
                    #region 注释
                    //var state = _informationBLL.AddInformation(InformationModel, currentUser.Account, currentUser.CompanyCode);
                    //if (state == OperatorState.empty)
                    //{
                    //    ar.state = ResultType.error.ToString();
                    //    ar.message = "提交的数据为空，添加新纪录失败";
                    //}
                    //else if (state == OperatorState.error)
                    //{
                    //    ar.state = ResultType.error.ToString();
                    //    ar.message = "添加新纪录失败";
                    //}
                    //else if (state == OperatorState.success)
                    //{
                    //    //ar.state = ResultType.success.ToString();
                    //    //ar.message = "添加新纪录成功";
                    //}
                    #endregion

                    using (var db = new DCSDBContext())
                    {
                        #region 开始一个事务
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                InformationModel.InsertMember = InformationModel.UsageMember = currentUser.Account;
                                InformationModel.State = (int)InformatinState.UnAssigned;
                                InformationModel.InsertTime = DateTime.Now;
                                InformationModel.UpdateTime = DateTime.Now;
                                InformationModel.CompanyCode = currentUser.CompanyCode;

                                db.Informations.Add(InformationModel);

                                List<CustomItem> customItemList = new List<CustomItem>();
                                _customItemBLL.GetCustomItems(currentUser.Account, ref customItemList);

                                foreach (var item in customItemList)
                                {
                                    var cm = CustomItemModel.Where(n => n.name == item.ItemName).SingleOrDefault();
                                    CustomItemValue cv = new CustomItemValue();

                                    cv.InforId = InformationModel.Id;
                                    cv.InsertTime = cv.UpdateTime = DateTime.Now;
                                    cv.IsDeleted = false;
                                    cv.ItemValue = cm.value;
                                    cv.CustomItemId = item.Id;

                                    db.CustomItemValues.Add(cv);
                                }

                                db.SaveChanges();
                                trans.Commit();

                                ar.state = ResultType.success.ToString();
                                ar.message = "添加成功";
                            }
                            catch (Exception ex)
                            {
                                LogHelper.writeLog_error(ex.Message);
                                LogHelper.writeLog_error(ex.StackTrace);

                                trans.Rollback();
                                ar.state = ResultType.error.ToString();
                                ar.message = "添加失败，数据已回滚";
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "已存在相同记录，添加新纪录失败";
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，添加新纪录失败";
            }
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判斷是否是同級
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private OperatorState IsSameRange(string parentName, int role)
        {
            try
            {
                var member = _memberBLL.GetUserByAccount(parentName);
                if (member == null)
                {
                    return OperatorState.empty;
                }

                if (member.Role == role)
                {
                    return OperatorState.error;
                }
                else
                {
                    return OperatorState.success;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }
    }

}