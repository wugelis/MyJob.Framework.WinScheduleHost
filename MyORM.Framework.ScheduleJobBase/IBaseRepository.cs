using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Framework.ScheduleJobBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T>
    {
        //IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Returns all the rows for type T
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Does this item exist by it's primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        bool Exists(Func<T, bool> exp);

        /// <summary>
        /// Inserts the data into the table
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        /// <param name="userId">The user performing the insert</param>
        /// <returns></returns>
        bool Insert(T entity);

        /// <summary>
        /// Updates this entity in the database using it's primary key
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">The user performing the update</param>
        bool Update(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        bool InsertOrUpdate(T entity);

        /// <summary>
        /// Deletes this entry fro the database
        /// ** WARNING - Most items should be marked inactive and Updated, not deleted
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="userId">The user Id who deleted the entity</param>
        /// <returns></returns>
        bool Delete(T entity);
    }
}
