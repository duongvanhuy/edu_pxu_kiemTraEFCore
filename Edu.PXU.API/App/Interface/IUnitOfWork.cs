﻿using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore;

namespace Edu.PXU.API.App.Interface
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IImageRepository ImageRepository { get; }


        void SaveChanges();
        void CreateTransaction();
        void Commit();
        void Rollback();
    }
}
