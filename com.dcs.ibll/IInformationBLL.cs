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
        bool IsExsit(string phone, string qq, string webchat);

        OperatorState AddInformation(Information information);

        IEnumerable<Information> GetInformationByCompany(string company);
    }
}
