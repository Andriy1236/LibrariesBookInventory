using LibrariesBookInventory.DomainServices.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibrariesBookInventory.Domain.Models;
using LibrariesBookInventory.Tests.Repositories.Helpers;

namespace LibrariesBookInventory.Tests.Repositories
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        private CategoryRepository _categoryRepository;
        private Mock<DbContext> _dbContextMock;
        private Mock<DbSet<Category>> _dbSetMock;

        [TestInitialize]
        public void Setup()
        {
            _dbContextMock = new Mock<DbContext>();
            _dbSetMock = new Mock<DbSet<Category>>();
            _dbContextMock.Setup(c => c.Set<Category>()).Returns(_dbSetMock.Object);
            _categoryRepository = new CategoryRepository(_dbContextMock.Object);
        }

        [TestMethod]
        public async Task Create_AddsCategoryToDatabase()
        {
            var category = new Category { Id = 1, Name = "Scientific", Description = "Scientific" };

            _categoryRepository.Create(category);
            await _categoryRepository.SaveChanges(new CancellationToken());
            _dbSetMock.Verify(mock => mock.Add(category), Times.Once);
        }

        [TestMethod]
        public async Task Delete_RemovesCategoryFromDatabase()
        {
            var cancellationToken = new CancellationToken();
            var category = new Category { Id = 1, Name = "Scientific", Description = "Scientific" };
            _dbSetMock.Setup(mock => mock.FindAsync(cancellationToken, category.Id)).ReturnsAsync(category);

            await _categoryRepository.Delete(category.Id, cancellationToken);

            _dbSetMock.Verify(mock => mock.Remove(category), Times.Once);
        }
        
        [TestMethod]
        public async Task Get_ReturnsCategoryFromDatabase()
        {
            var cancellationToken = new CancellationToken();
            var category = new Category { Id = 1, Name = "Scientific", Description = "Scientific" };
            _dbSetMock.Setup(mock => mock.FindAsync(cancellationToken, category.Id)).ReturnsAsync(category);

            var result = await _categoryRepository.Get(category.Id, cancellationToken);

            Assert.AreEqual(category, result);
        }

        [TestMethod]
        public async Task GetAll_ReturnsAllCategoriesFromDatabase()
        {
            IEnumerable<Category> categories = new List<Category>
            {
                new Category { Id = 1, Name = "Scientific", Description = "Scientific" },
                new Category { Id = 1, Name = "Artistic", Description = "Artistic" }
            };

            var context = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<Category>>();
            dbSetMock.As<IQueryable<Category>>().Setup(x => x.Provider).Returns(categories.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Category>>().Setup(x => x.Expression).Returns(categories.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Category>>().Setup(x => x.ElementType).Returns(categories.AsQueryable().ElementType);
            dbSetMock.As<IDbAsyncEnumerable<Category>>().Setup(x => x.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Category>(categories.GetEnumerator()));

            context.Setup(x => x.Set<Category>()).Returns(dbSetMock.Object);

            _categoryRepository = new CategoryRepository(context.Object);
            var result = await _categoryRepository.GetAll(new CancellationToken());

            CollectionAssert.AreEqual(categories.ToList(), result.ToList());
        }
    }
}
