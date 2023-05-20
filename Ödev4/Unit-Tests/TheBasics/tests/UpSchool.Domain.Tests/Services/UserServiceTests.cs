using FakeItEasy;
using UpSchool.Domain.Data;
using UpSchool.Domain.Entities;
using UpSchool.Domain.Services;

namespace UpSchool.Domain.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUser_ShouldGetUserWithCorrectId()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();

            Guid userId = new Guid("8f319b0a-2428-4e9f-b7c6-ecf78acf00f9");

            var cancellationSource = new CancellationTokenSource();

            var expectedUser = new User()
            {
                Id = userId
            };

            A.CallTo(() => userRepositoryMock.GetByIdAsync(userId, cancellationSource.Token))
                .Returns(Task.FromResult(expectedUser));

            IUserService userService = new UserManager(userRepositoryMock);

            var user = await userService.GetByIdAsync(userId, cancellationSource.Token);

            Assert.Equal(expectedUser, user);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenEmailIsEmptyOrNull()
        {
            //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();

            IUserService userService = new UserManager(userRepositoryMock);

            //Act
            var exceptionForNullEmail = Assert.ThrowsAsync<ArgumentException>(() => userService.AddAsync("TestName", "TestLastName", 20, null, cancellationSource.Token));
            var exceptionForEmptyEmail = Assert.ThrowsAsync<ArgumentException>(() => userService.AddAsync("TestName", "TestLastName", 20, string.Empty, cancellationSource.Token));

            //Assert
            Assert.Equal("Email cannot be null or empty.", exceptionForNullEmail.Result.Message);
            Assert.Equal("Email cannot be null or empty.", exceptionForEmptyEmail.Result.Message);

        }

        [Fact]
        public async Task AddAsync_ShouldReturn_CorrectUserId()
        {
            //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();

            IUserService userService = new UserManager(userRepositoryMock);

            //Act
            var userHasUserId = await userService.AddAsync("TestName", "TestLastName", 20, "test@gmail.com", cancellationSource.Token);

            //Assert
            Assert.IsType<Guid>(userHasUserId);
            Assert.NotNull(userHasUserId);
            Assert.NotEqual(Guid.Empty, userHasUserId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenUserExists()
        {
            //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();

            IUserService userService = new UserManager(userRepositoryMock);

            var expectedUserId = await userService.AddAsync("TestName", "TestLastName", 20, "test@gmail.com", cancellationSource.Token);

            //A.CallTo(() => userRepositoryMock.DeleteAsync(expectedUserId, cancellationSource.Token))
            //    .Returns(Task.FromResult(true));

            //Act
            var resultOfDeleteAsync = await userService.DeleteAsync(expectedUserId, cancellationSource.Token);

            //Assert
            Assert.True(resultOfDeleteAsync);

        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesntExists()
        {
            //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();

            IUserService userService = new UserManager(userRepositoryMock);

            //Act
            var resultOfDeleteAsync = Assert.ThrowsAsync<ArgumentException>(() => userService.DeleteAsync(Guid.Empty, cancellationSource.Token));

            //Assert
            Assert.Equal("id cannot be empty.", resultOfDeleteAsync.Result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserIdIsEmpty()
        {
            //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();

            var userWithEmptyId = new User()
            {
                Id = Guid.Empty,
                FirstName = "TestName",
                LastName = "TestLastName",
                Age = 20,
                Email = "test@gmail.com"
            };

            IUserService userService = new UserManager(userRepositoryMock);

            //Act
            var resultOfEmptyIdUser = Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(userWithEmptyId, cancellationSource.Token));

            //Assert
            Assert.Equal("User Id cannot be null or empty.", resultOfEmptyIdUser.Result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserEmailEmptyOrNull()
        {
            //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();

            var userWithEmptyEmail = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "TestName",
                LastName = "TestLastName",
                Age = 20,
                Email = string.Empty
            };

            var userWithNullEmail = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "TestName",
                LastName = "TestLastName",
                Age = 20,
                Email = null
            };

            IUserService userService = new UserManager(userRepositoryMock);

            //Act
            var resultOfEmptyEmailUser = Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(userWithEmptyEmail, cancellationSource.Token));

            var resultOfNullEmailUser = Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(userWithNullEmail, cancellationSource.Token));

            //Assert
            Assert.Equal("Email cannot be null or empty.", resultOfEmptyEmailUser.Result.Message);
            Assert.Equal("Email cannot be null or empty.", resultOfNullEmailUser.Result.Message);

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturn_UserListWithAtLeastTwoRecords()
        {
            //Arrange
            var userRepositoryMock = A.Fake<IUserRepository>();

            var cancellationSource = new CancellationTokenSource();

            var userList = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "User1",
                    LastName = "User1",
                    Age = 20,
                    Email = "user1@gmail.com"
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "User2",
                    LastName = "User2",
                    Age = 20,
                    Email = "user2@gmail.com"
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "User3",
                    LastName = "User3",
                    Age = 20,
                    Email = "user3@gmail.com"
                },

            };

            A.CallTo(() => userRepositoryMock.GetAllAsync(cancellationSource.Token))
                .Returns(Task.FromResult(userList));

            IUserService userService = new UserManager(userRepositoryMock);

            //Act
            var resultOfGetAllAsync = await userService.GetAllAsync(cancellationSource.Token);

            //Assert
            Assert.NotEmpty(resultOfGetAllAsync);
            Assert.True(resultOfGetAllAsync.Count >= 2);

        }
    }
}
