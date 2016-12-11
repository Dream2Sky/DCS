using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.common;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var currentTime = DateTime.Now;
            //var mcode = GetHardDiskID();
            //var millsce = DateTime.Now.Millisecond;


            //Console.WriteLine(currentTime);
            //Console.WriteLine(mcode);

            DCSDBContext db = new DCSDBContext();

            Company comp = new Company();
            comp.CompanyCode = "A";
            comp.CompanyName = "东方宝源";
            comp.InsertTime = DateTime.Now;
            comp.IsDeleted = false;
            comp.UpdateTime = DateTime.Now;

            db.Companies.Add(comp);

            Member member = new Member();
            member.Account = "Li";
            member.CompanyCode = comp.CompanyCode;
            member.InsertTime = DateTime.Now;
            member.IsDeleted = false;
            member.Name = "李秋颖";
            member.Parent = "SuperAdmin";
            member.Password = EncryptManager.SHA1("123456");
            member.Role = 0;
            member.UpdateTime = DateTime.Now;

            db.Members.Add(member);

            db.SaveChanges();

            //Console.WriteLine(EncryptManager.SHA1("admin"));

            Console.WriteLine("finish");
            Console.ReadKey();
        }

        //取第一块硬盘编号
        public static string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = null;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return strHardDiskID;
            }
            catch
            {
                return "";
            }
        }//end
    }
}
