using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.common;

namespace com.dcs.ibll
{
    public interface IInformationBLL
    {
        bool IsExsit(string phone, string qq, string webchat, string companycode);

        OperatorState AddInformation(Information information, string user , string companycode);

        IEnumerable<Information> GetInformationByCompany(string company);

        OperatorState GetInformation(string member, InformatinState state, ref List<InformationModel> modelList);

        OperatorState GetInformation(Member member, ref List<InformationModel> modelList);

        Information GetInformation(string dataCode);

        OperatorState GetInformation(string keyword, string member,ConditionModal conditionModal, List<CustomItem> customItemList,  ref List<InformationModel> modelList);

        List<Information> GetInformation(List<InformationModel> modelList);

        
        //OperatorState GetInformation(string conditions,Member member, ref List<InformationModel> modelList);
    }
}
