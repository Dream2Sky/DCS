using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.common;

namespace com.dcs.idal
{
    public interface IInformationDAL:IDataBaseDAL<Information>
    {
        bool IsExist(string phone, string qq, string webchat, string companycode);
        IEnumerable<InformationModel> SelectByConditions(string member, InformatinState state);
        IEnumerable<InformationModel> SelectByConditions(Member member);
    }
}
