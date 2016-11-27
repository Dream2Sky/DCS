using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.idal;
using com.dcs.entity;
using System.Data.Entity;
using com.dcs.common;

namespace com.dcs.dal
{
    public class DataBaseDAL<T> : IDataBaseDAL<T> where T : Entity
    {
        protected readonly DCSDBContext db;
        public DataBaseDAL()
        {
            db = new DCSDBContext();
        }

        public bool Clear()
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var entitys = db.Set<T>();
                    entitys.ToList().ForEach(entity => db.Entry(entity).State = System.Data.Entity.EntityState.Deleted); //不加这句也可以，为什么？
                    db.Set<T>().RemoveRange(entitys);

                    SaveChanges();
                    trans.Rollback();
                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;

                }
            }
        }

        public bool Delete(T t)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Set<T>().Attach(t);
                    db.Entry(t).State = EntityState.Deleted;

                    SaveChanges();

                    trans.Commit();

                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }

        public bool Insert(T t)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Set<T>().Attach(t);
                    db.Set<T>().Add(t);

                    SaveChanges();
                    trans.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.writeLog_error(ex.Message);
                    LogHelper.writeLog_error(ex.StackTrace);
                    trans.Rollback();
                    return false;
                }
            }
        }

        public IEnumerable<T> SelectAll()
        {
            try
            {
                return db.Set<T>();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        public T SelectById(Guid id)
        {
            try
            {
                return db.Set<T>().SingleOrDefault(n => n.Id == id);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        public bool Update(T t)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Set<T>().Attach(t);
                    db.Entry(t).State = EntityState.Modified;

                    SaveChanges();

                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.writeLog_error(ex.Message);
                    LogHelper.writeLog_error(ex.StackTrace);
                    return false;
                }
            }
        }

        private int SaveChanges()
        {
            try
            {
                int result = db.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                string message = "error:";
                if (ex.InnerException == null)
                    message += ex.Message + ",";
                else if (ex.InnerException.InnerException == null)
                    message += ex.InnerException.Message + ",";
                else if (ex.InnerException.InnerException.InnerException == null)
                    message += ex.InnerException.InnerException.Message + ",";

                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw new Exception(message);
            }
        }

        public void ReStartConnection()
        {
            if (db.Database.Connection.State == System.Data.ConnectionState.Open)
            {
                db.Database.Connection.Close();
                db.Database.Connection.Open();
            }
        }
    }
}
