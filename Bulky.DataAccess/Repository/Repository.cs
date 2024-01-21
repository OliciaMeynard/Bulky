﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db; 
            this.dbSet = _db.Set<T>();
            //_db.Categories == dbSet

            //this code also works, which means if you get products Table you also get Categories table
            //_db.Products.Include(u => u.Category);
            //_db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
            //_db.Categories.Add(sampleentity)

        }



        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeprop in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                    ///just likde these 
                    //_db.Products.Include(u => u.Category);
                    //_db.Products.Include(u => u.Category).Include(u => u.CategoryId);
                }
            }
            return query.FirstOrDefault();
            //Category? categoryFromDb = _db.Categories.Where(cat => cat.Id == id).FirstOrDefault();
        }

        ///
        //public IEnumerable<T> GetAll()
        //{
        //    IQueryable<T> query = dbSet;
        //    return query.ToList();
        //}


        //these is example
        //Category, Covertype
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeprop in includeProperties
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                    ///just likde these 
                    //_db.Products.Include(u => u.Category);
                    //_db.Products.Include(u => u.Category).Include(u => u.CategoryId);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
