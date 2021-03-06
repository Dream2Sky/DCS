﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.dcs.entity;
using com.dcs.common;
using com.dcs.ibll;
using System.Web.Mvc;
using com.dcs.web.Globals;
using com.dcs.web.Models;
using System.IO;

namespace com.dcs.web.Controllers
{
    [IsLogin]
    public class CompetentController : Controller
    {
        private IMemberBLL _memberBLL;
        private UnderlingManager _underlingManager;
        private ICustomItemBLL _customItemBLL;
        private IInformationBLL _informationBLL;
        private ICustomItemValueBLL _customItemValueBLL;
        private ExcelManager _excelManager;
        private DataManager _dataManager;
        public CompetentController(IMemberBLL memberBLL, UnderlingManager underlingManager, 
            ICustomItemBLL customItemBLL, IInformationBLL informationBLL, 
            ICustomItemValueBLL customItemValueBLL,ExcelManager excelManager, DataManager dataManager)
        {
            _memberBLL = memberBLL;
            _underlingManager = underlingManager;
            _customItemBLL = customItemBLL;
            _informationBLL = informationBLL;
            _customItemValueBLL = customItemValueBLL;
            _excelManager = excelManager;
            _dataManager = dataManager;
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

        #region 数据查询操作

        public ActionResult SearchPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(bool OnlyKeyword, ConditionModal conditionModel, string keyword)
        {
            AjaxResult ar = new Globals.AjaxResult();
            if (conditionModel == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的数据为空，查询数据失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            var currentUser = LoginManager.GetCurrentUser();
            List<CustomItem> customItemList = new List<CustomItem>();
            List<InformationModel> modelList = new List<InformationModel>();

            try
            {
                var state = OperatorState.error;
                
                List<Member> memberList =_underlingManager.GetUnderlingList();

                foreach (var member in memberList)
                {
                    state = _customItemBLL.GetCustomItems(member.Account, ref customItemList);
                    if (state == OperatorState.error)
                    {
                        ar.state = ResultType.error.ToString();
                        ar.message = "获取自定义项失败，无法搜索到数据";

                        return Json(ar, JsonRequestBehavior.AllowGet);
                    }

                    // 是否使用条件查询模式
                    if (OnlyKeyword)
                    {
                        state = _informationBLL.GetInformation(keyword, member.Account, conditionModel, customItemList, ref modelList);
                    }
                    else
                    {
                        state = _informationBLL.GetInformation(keyword, member.Account, null, customItemList, ref modelList);
                    }
                }

                // 查询自己
                state = _customItemBLL.GetCustomItems(currentUser.Account, ref customItemList);
                if (state == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "获取自定义项失败，无法搜索到数据";

                    return Json(ar, JsonRequestBehavior.AllowGet);
                }

                // 是否使用条件查询模式
                if (OnlyKeyword)
                {
                    state = _informationBLL.GetInformation(keyword, currentUser.Account, conditionModel, customItemList, ref modelList);
                }
                else
                {
                    state = _informationBLL.GetInformation(keyword, currentUser.Account, null, customItemList, ref modelList);
                }

                if (state == OperatorState.empty)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "无法获取到当前用户，无法搜索到数据";
                }
                else if (state == OperatorState.success)
                {
                    if (modelList == null || modelList.Count <= 0)
                    {
                        ar.state = ResultType.error.ToString();
                        ar.message = "无法搜索到有关数据";
                    }
                    else
                    {
                        ar.state = ResultType.success.ToString();
                        ar.data = modelList.ToJson();

                        DataCacheManager.SetDataCache(CachaKey.Key, modelList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，获取数据失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchCache()
        {
            AjaxResult ar = new AjaxResult();
            try
            {
                List<InformationModel> modelList = DataCacheManager.GetDataCache(CachaKey.Key);
                ar.state = ResultType.success.ToString();
                ar.data = modelList.ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "获取缓存数据失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 获取角色列表 当前角色下面有权限添加的角色列表
        /// 比如管理员 有权添加 收集员 和 普通员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRoles()
        {
            AjaxResult ar = new AjaxResult();
            try
            {
                var currentUser = LoginManager.GetCurrentUser();
                var code = currentUser.Role;
                var parent = _memberBLL.GetUserByAccount(currentUser.Parent);

                List<RolesModel> rmList = RolesManager.GetRolesList().Where(n => n.Code != code && n.Code != parent.Role).ToList();

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
                    _underlingManager.AddUnderlingList(member);
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

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

                var currentUser = LoginManager.GetCurrentUser();

                var result = _memberBLL.UpdateMember(model.Account, model.Name, model.RoleCode, currentUser.Account);

                if (result == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "修改該用戶失敗";
                }
                else if (result == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                    ar.message = "修改該用戶成功";

                    _underlingManager.UpdateUnderlingList(member);
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

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
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
                    _underlingManager.SubUnderlingList(member);
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
                                InformationModel.DataCode = "DC" + currentUser.CompanyCode + TimeManager.GetTimeSpan() + RandomManager.GenerateRandom(5);

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
                                    cv.ItemValue = cm.value ?? "";
                                    cv.CustomItemId = item.Id;
                                    cv.ItemName = item.ItemName;

                                    db.CustomItemValues.Add(cv);
                                }

                                db.SaveChanges();

                                // 修改用户的以收集数据的数量  + 1
                                currentUser.Cocount += 1;
                                db.Set<Member>().Attach(currentUser);
                                db.Entry(currentUser).State = System.Data.Entity.EntityState.Modified;

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

        public ActionResult DetailPage(string datacode)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Detail(string datacode)
        {
            AjaxResult ar = new AjaxResult();
            if (datacode == string.Empty)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的数据为空，获取数据失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var information = _informationBLL.GetInformation(datacode);
                if (information == null)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "不存在相应的数据";
                    return Json(ar, JsonRequestBehavior.AllowGet);
                }

                ar.state = ResultType.success.ToString();
                ar.data = information.ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，获取数据失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCustomItemValues(string datacode)
        {
            AjaxResult ar = new Globals.AjaxResult();

            if (datacode == string.Empty)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的数据为空，获取自定义项失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                // 获取当前登录的用户
                var currentUser = LoginManager.GetCurrentUser();

                List<CustomItemValue> itemvalueList = new List<CustomItemValue>();

                // 根据datacode获取到相应的数据
                var infor = _informationBLL.GetInformation(datacode);

                // 获取自定义值列表
                var state = _customItemValueBLL.GetCustomItemValueByMember(currentUser.Account, infor.Id, ref itemvalueList);

                if (state == OperatorState.empty)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "无法获取到当前用户信息，获取数据失败";
                }
                else if (state == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "获取数据失败";
                }
                else if (state == OperatorState.success)
                {
                    List<CustomItemModel> cmList = ModelChangeManager.ChangeTOCustomItemModel(itemvalueList);

                    ar.state = ResultType.success.ToString();
                    ar.data = cmList.ToJson();
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，获取数据失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="datacode"></param>
        /// <param name="InformationModel"></param>
        /// <param name="CustomItemModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateInformation(string datacode, Information InformationModel, List<FormModel> CustomItemModel)
        {
            AjaxResult ar = new Globals.AjaxResult();
            if (datacode == string.Empty || InformationModel == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交的数据为空";
                return Json(ar, JsonRequestBehavior.AllowGet);
            }
            var currentUser = LoginManager.GetCurrentUser();
            using (var db = new DCSDBContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var item = db.Informations.SingleOrDefault(n => n.DataCode == datacode);

                        item.Address = InformationModel.Address;
                        item.Age = InformationModel.Age;
                        item.Children = InformationModel.Children;
                        item.CustomerName = InformationModel.CustomerName;
                        item.Email = InformationModel.Email;
                        item.HasCar = InformationModel.HasCar;
                        item.HasHouse = InformationModel.HasHouse;
                        item.Hobby = InformationModel.Hobby;
                        item.Income = InformationModel.Income;
                        item.Industry = InformationModel.Industry;
                        item.InvestConc = InformationModel.InvestConc;
                        item.InvestLife = InformationModel.InvestLife;
                        item.InvestProj = InformationModel.InvestProj;
                        item.IsMarry = InformationModel.IsMarry;
                        item.Note1 = InformationModel.Note1;
                        item.Note2 = InformationModel.Note2;
                        item.Note3 = InformationModel.Note3;
                        item.Occupation = InformationModel.Occupation;
                        item.Phone = InformationModel.Phone;
                        item.QQ = InformationModel.QQ;
                        item.Sex = InformationModel.Sex;
                        item.UpdateTime = DateTime.Now;
                        item.WebCat = InformationModel.WebCat;

                        //db.Informations.Attach(InformationModel);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();

                        List<CustomItem> cmList = new List<CustomItem>();
                        _customItemBLL.GetCustomItems(currentUser.Account, ref cmList);
                        foreach (var cmItem in cmList)
                        {
                            var customItemValue = _customItemValueBLL.GetCustomItemValueByCustomItemIdAndInforId(cmItem.Id, item.Id);
                            var cm = CustomItemModel.Where(n => n.name == cmItem.ItemName).SingleOrDefault();
                            if (cm != null)
                            {
                                customItemValue.ItemValue = cm.value;
                                customItemValue.ItemName = cmItem.ItemName;
                                //db.CustomItemValues.Attach(customItemValue);
                                db.Entry(customItemValue).State = System.Data.Entity.EntityState.Modified;
                            }
                        }

                        db.SaveChanges();

                        trans.Commit();

                        ar.state = ResultType.success.ToString();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        LogHelper.writeLog_error(ex.Message);
                        LogHelper.writeLog_error(ex.StackTrace);

                        ar.state = ResultType.error.ToString();
                        ar.message = "系统错误，修改数据失败";
                    }
                }
            }
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分配数据
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public ActionResult AssignData(string Account)
        {
            ViewBag.Member = Account;
            return View();
        }

        /// <summary>
        /// 获取当前用户与收集员的可以分配的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadEnableAssignData()
        {
            AjaxResult ar = new Globals.AjaxResult();

            var currentUser = LoginManager.GetCurrentUser();

            try
            {
                var currentUserData = new List<InformationModel>();
                var collectorData = new List<InformationModel>();

                var currentUserState = _informationBLL.GetInformation(currentUser.Account, InformatinState.UnAssigned, ref currentUserData);
                foreach (var item in _memberBLL.GetUsersByRole(RolesManager.GetRolesCode(RolesCode.Collector.ToString()), currentUser.CompanyCode))
                {
                    var temp = new List<InformationModel>();
                    var tempstate = _informationBLL.GetInformation(item.Account, InformatinState.UnAssigned, ref temp);
                    collectorData.AddRange(temp);
                }

                currentUserData.AddRange(collectorData);
                if (currentUserData == null || currentUserData.Count <= 0)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "没有获取到可供分配的数据";
                }
                else
                {
                    ar.state = ResultType.success.ToString();
                    ar.data = currentUserData.ToJson();
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误，无法获取到数据";
            }
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分配数据给指定的用户
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="DataCodeList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Assign(string Account, string[] DataCodeList)
        {
            AjaxResult ar = new Globals.AjaxResult();
            if (Account == string.Empty)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "分配的用户为空，分配失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            if (DataCodeList == null || DataCodeList.Length <= 0)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "分配的数据为空，请选择要分配的数据";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }
            var member = _memberBLL.GetUserByAccount(Account);
            if (member == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "无法获取到相应的用户，分配失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            #region 分配数据 给指定 的 用户

            using (var db = new DCSDBContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var count = 0;
                        List<Information> inforList = new List<Information>();
                        foreach (var item in DataCodeList)
                        {
                            var temp = _informationBLL.GetInformation(item);
                            if (temp != null)
                            {
                                count++;
                                temp.UsageMember = Account;
                                temp.State = (int)InformatinState.Assigned;
                                inforList.Add(temp);
                            }
                        }

                        foreach (var item in inforList)
                        {
                            db.Informations.Attach(item);
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        }

                        ///int count = db.Informations.Update(inforList.AsQueryable(), n => new Information { UsageMember = Account});

                        //更新 已分配数据的数量
                        member.Ascount += count;
                        db.Members.Attach(member);
                        db.Entry(member).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();

                        trans.Commit();

                        // 更新下属名单
                        _underlingManager.UpdateUnderlingList(member);

                        ar.state = ResultType.success.ToString();
                        ar.message = "分配成功";


                    }
                    catch (Exception ex)
                    {
                        LogHelper.writeLog_error(ex.Message);
                        LogHelper.writeLog_error(ex.StackTrace);

                        trans.Rollback();
                        ar.state = ResultType.error.ToString();
                        ar.message = "分配失败";
                    }
                }
            }
            #endregion
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckMember(string Account)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Check(string Account)
        {
            AjaxResult ar = new AjaxResult();
            Member member = new Member();
            try
            {
                member = _memberBLL.GetUserByAccount(Account);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "无法获取当前用户";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            try
            {
                List<InformationModel> modelList = new List<InformationModel>();

                var state = _informationBLL.GetInformation(member, ref modelList);

                if (state == OperatorState.error)
                {
                    ar.state = ResultType.error.ToString();
                    ar.message = "获取当前用户数据失败";

                }
                else if (state == OperatorState.success)
                {
                    ar.state = ResultType.success.ToString();
                    ar.data = modelList;
                }

            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "系统错误， 获取当前用户数据失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="fileCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] fileCollection)
        {
            AjaxResult ar = new Globals.AjaxResult();

            if (fileCollection.First() == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "上传的文件为空，请重新上传";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            var currentUser = LoginManager.GetCurrentUser();
            List<string> fileList = new List<string>();

            fileList = FileManager.SaveFile(fileCollection, Server.MapPath("~/Files"));

            List<Information> informationList = new List<Information>();
            List<CustomItem> customItemList = new List<CustomItem>();
            _customItemBLL.GetCustomItems(currentUser.Account, ref customItemList);
            List<CustomItemValue> customItemValueList = new List<CustomItemValue>();

            try
            {
                foreach (var item in fileList)
                {
                    List<Information> tempInforList = new List<Information>();

                    _excelManager.Open(item);
                    _excelManager.GetDataFromExcel(currentUser.Account, ref tempInforList, ref customItemList, ref customItemValueList);
                    _excelManager.Close(); //关闭并删除文件

                    informationList.AddRange(tempInforList);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                ar.state = ResultType.error.ToString();
                ar.message = "解析数据失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            bool state = _dataManager.ExcelToDataBase(informationList, currentUser, customItemList, customItemValueList);
            if (state)
            {
                ar.state = ResultType.success.ToString();
                ar.message = "导入成功";
            }
            else
            {
                ar.state = ResultType.error.ToString();
                ar.message = "导入数据失败";
            }

            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出数据到Excel表 导出的excel表为 .xls 格式 
        /// </summary>
        /// <returns></returns>
        public FileResult ExportData()
        {
            var currentUser = LoginManager.GetCurrentUser();
            try
            {
                List<Information> informationList = _informationBLL.GetInformation(DataCacheManager.GetDataCache(CachaKey.Key));
                List<CustomItem> customItemList = new List<CustomItem>();
                _customItemBLL.GetCustomItems(currentUser.Account, ref customItemList);
                List<CustomItemValue> customItemValueList = new List<CustomItemValue>();

                foreach (var item in informationList)
                {
                    foreach (var ci in customItemList)
                    {
                        var tmp = _customItemValueBLL.GetCustomItemValueByCustomItemIdAndInforId(ci.Id, item.Id);

                        if (tmp != null)
                        {
                            customItemValueList.Add(tmp);
                        }
                    }
                }

                MemoryStream ms = _excelManager.DataTOExcel(informationList, customItemList, customItemValueList);

                return File(ms, "application/vnd.ms-excel", TimeManager.GetTimeSpan() + ".xls");
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }

        }

        /// <summary>
        /// 导出所有数据
        /// </summary>
        /// <returns></returns>
        public FileResult ExportAll()
        {
            var currentUser = LoginManager.GetCurrentUser();
            try
            {
                List<Information> informationList = new List<Information>();
                List<InformationModel> modelList = new List<InformationModel>();
                _informationBLL.GetInformation(currentUser, ref modelList);
                informationList = _informationBLL.GetInformation(modelList);

                List<CustomItem> customItemList = new List<CustomItem>();
                _customItemBLL.GetCustomItems(currentUser.Account, ref customItemList);
                List<CustomItemValue> customItemValueList = new List<CustomItemValue>();

                foreach (var item in informationList)
                {
                    foreach (var ci in customItemList)
                    {
                        var tmp = _customItemValueBLL.GetCustomItemValueByCustomItemIdAndInforId(ci.Id, item.Id);

                        if (tmp != null)
                        {
                            customItemValueList.Add(tmp);
                        }
                    }
                }

                MemoryStream ms = _excelManager.DataTOExcel(informationList, customItemList, customItemValueList);

                return File(ms, "application/vnd.ms-excel", TimeManager.GetTimeSpan() + ".xls");
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approval(string dataCode)
        {
            AjaxResult ar = new AjaxResult();
            var currentUser = LoginManager.GetCurrentUser();

            if (string.IsNullOrEmpty(dataCode))
            {
                ar.state = ResultType.error.ToString();
                ar.message = "提交数据为空，审批失败";

                return Json(ar, JsonRequestBehavior.AllowGet);
            }

            var information = _informationBLL.GetInformation(dataCode);
            if (information == null)
            {
                ar.state = ResultType.error.ToString();
                ar.message = "不存在相应的数据，审批失败";
            }
            else
            {
                information.State = (int)InformatinState.UnAssigned;
                var usageMember = information.UsageMember;

                information.UsageMember = currentUser.Account;

                using (var db = new DCSDBContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Set<Information>().Attach(information);
                            db.Entry(information).State = System.Data.Entity.EntityState.Modified;

                            var member = db.Set<Member>().Where(n => n.Account == usageMember).SingleOrDefault();
                            member.Apcount -= 1;
                            member.Ascount -= 1;

                            db.Set<Member>().Attach(member);
                            db.Entry(member).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();

                            trans.Commit();

                            _underlingManager.UpdateUnderlingList(member);

                            ar.state = ResultType.success.ToString();
                            ar.message = "审批成功";
                        }
                        catch (Exception ex)
                        {
                            LogHelper.writeLog_error(ex.Message);
                            LogHelper.writeLog_error(ex.StackTrace);

                            trans.Rollback();
                            ar.state = ResultType.error.ToString();
                            ar.message = "系统错误，审批失败";
                        }
                    }
                }
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