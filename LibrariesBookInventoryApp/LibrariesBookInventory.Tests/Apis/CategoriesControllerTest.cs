using LibrariesBookInventory.Application.Category;
using LibrariesBookInventory.Application.Category.Create;
using LibrariesBookInventory.Application.Category.Update;
using LibrariesBookInventory.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Tests.Apis
{
    [TestClass]
    public class CategoriesControllerTest : BaseControllerTest
    {
        protected override string BaseApi => "api/categories";

        [TestMethod]
        public async Task Get_Category_By_Not_Exist_Id_Error()
        {
            var categoryId = await GetMaxId() + 1;
            var endPoint = $"/{categoryId}";

            var response = await Get(endPoint);

            var content = await response.Content.ReadAsStringAsync();

            var error = JsonConvert.DeserializeObject<string>(content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            Assert.IsTrue(error == "Category id is not exist");
        }

        [TestMethod]
        public async Task Get_All_Categories_Success()
        {
            var category1 = new Category() { Name = Guid.NewGuid().ToString(), Description = "Description1" };
            var category2 = new Category() { Name = Guid.NewGuid().ToString(), Description = "Description2" };
            
            Db.Categories.Add(category1);
            Db.Categories.Add(category2);
            await Db.SaveChangesAsync();

            var endPoint = "";

            var response = await Get(endPoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(content).ToList();

            Assert.AreEqual(2, categories.Count);
            Assert.IsTrue(categories.Any(x => x.Id == category1.Id));
            Assert.IsTrue(categories.Any(x => x.Id == category2.Id));
        }

        [TestMethod]
        public async Task Get_Category_By_Id_Success()
        {
            var dbCategory = new Category() { Name = Guid.NewGuid().ToString(), Description = "Description1" };
            Db.Categories.Add(dbCategory);
            await Db.SaveChangesAsync();

            var endPoint = $"/{dbCategory.Id}";

            var response = await Get(endPoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            var category = JsonConvert.DeserializeObject<CategoryDto>(content);

            Assert.IsTrue(dbCategory.Id == category.Id);
            Assert.IsTrue(dbCategory.Name == category.Name);
            Assert.IsTrue(dbCategory.Description == category.Description);
        }

        [TestMethod]
        public async Task Delete_Category_By_Not_Exist_Id_Error()
        {
            var categoryId = await GetMaxId() + 1;
            var endPoint = $"/{categoryId}";

            var response = await Delete(endPoint);

            var content = await response.Content.ReadAsStringAsync();

            var error = JsonConvert.DeserializeObject<string>(content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            Assert.IsTrue(error == "Category id is not exist");
        }

        [TestMethod]
        public async Task Delete_Category_By_Id_Success()
        {
            var dbCategory = new Category() { Name = Guid.NewGuid().ToString(), Description = "Description1" };
            Db.Categories.Add(dbCategory);
            await Db.SaveChangesAsync();

            var endPoint = $"/{dbCategory.Id}";

            var response = await Delete(endPoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
            dbCategory = await Db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == dbCategory.Id);

            Assert.IsNull(dbCategory);
        }

        [TestMethod]
        public async Task Create_Category_Success()
        {
            var createCategoryCommand = new CreateCategoryCommand() { Name = Guid.NewGuid().ToString(), Description = "Description1" };

            var endPoint = "";

            var response = await Post(endPoint, createCategoryCommand);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<long>(content);

            var dbCategory = await Db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(dbCategory.Name, createCategoryCommand.Name);
            Assert.AreEqual(dbCategory.Description, createCategoryCommand.Description);
        }

        [TestMethod]
        public async Task Create_Invalid_Category_Error()
        {
            var createCategoryCommand = new CreateCategoryCommand();

            var endPoint = "";

            var response = await Post(endPoint, createCategoryCommand);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            var errors = JsonConvert.DeserializeObject<IEnumerable<string>>(content).ToList();

            Assert.IsTrue(errors.Contains("Please specify a Name"));
            Assert.IsTrue(errors.Contains("Please specify a Description"));

        }

        [TestMethod]
        public async Task Update_Category_Success()
        {
            var dbCategory = new Category() { Name = Guid.NewGuid().ToString(), Description = "Description1" };
            Db.Categories.Add(dbCategory);
            await Db.SaveChangesAsync();

            var endPoint = $"/{dbCategory.Id}";

            var updateCategoryCommand = new UpdateCategoryCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };
            var response = await Put(endPoint, updateCategoryCommand);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            dbCategory = await Db.Categories.AsNoTracking().FirstAsync(x => x.Id == dbCategory.Id);

            Assert.AreEqual(updateCategoryCommand.Name, dbCategory.Name);
            Assert.AreEqual(updateCategoryCommand.Description, dbCategory.Description);
        }

        [TestMethod]
        public async Task Update_Not_Exist_Category_Error()
        {
            var endPoint = $"/{await GetMaxId() + 1}";

            var updateCategoryCommand = new UpdateCategoryCommand();

            var response = await Put(endPoint, updateCategoryCommand);

            var content = await response.Content.ReadAsStringAsync();

            var error = JsonConvert.DeserializeObject<string>(content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            Assert.IsTrue(error == "Category id is not exist");
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            var categories = await Db.Categories.ToListAsync();
            Db.Categories.RemoveRange(categories);

            await Db.SaveChangesAsync();
        }

        private async Task<long> GetMaxId()
        {
            var maxId = await Db.Categories.MaxAsync(x => (long?)x.Id);

            return maxId ?? 0;
        }
    }
}