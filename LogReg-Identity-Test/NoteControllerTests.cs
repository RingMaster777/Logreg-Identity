//using LogReg_Identity.Controllers;
//using LogReg_Identity.Data;
//using LogReg_Identity.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System.Security.Claims;


//namespace LogReg_Identity_Test
//{
//    public class NoteControllerTests
//    {
//        private readonly NoteController _controller;
//        private readonly Mock<ApplicationDbContext> _dbContextMock;
//        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;

//        public NoteControllerTests()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            _dbContextMock = new Mock<ApplicationDbContext>(options);
//            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
//                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

//            _controller = new NoteController(_dbContextMock.Object, _signInManagerMock.Object);
//        }

//        // Test Index action when user is signed in
//        [Fact]
//        public async Task Index_UserIsSignedIn_ReturnsNotes()
//        {
//            // Arrange
//            var notes = new List<NoteModel> { new NoteModel { NoteId = 1, NoteTitle = "Test Note" } };
//            _signInManagerMock.Setup(sm => sm.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);
//           // _dbContextMock.Setup(db => db.Notes.ToListAsync()).ReturnsAsync(notes);

//            var context = new DefaultHttpContext();
//            context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "user-id") }));
//            _controller.ControllerContext.HttpContext = context;

//            // Act
//            var result = await _controller.Index() as ViewResult;

//            // Assert
//            var model = result?.Model as IEnumerable<NoteModel>;
//            Assert.NotNull(result);
//            Assert.NotNull(model);
//            Assert.Single(model);
//            Assert.Equal("Test Note", model.First().NoteTitle);
//        }

//        // Test Index action when user is not signed in
//        [Fact]
//        public async Task Index_UserIsNotSignedIn_ReturnsEmptyList()
//        {
//            // Arrange
//            _signInManagerMock.Setup(sm => sm.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

//            var context = new DefaultHttpContext();
//            context.User = new ClaimsPrincipal(new ClaimsIdentity());
//            _controller.ControllerContext.HttpContext = context;

//            // Act
//            var result = await _controller.Index() as ViewResult;

//            // Assert
//            var model = result?.Model as IEnumerable<NoteModel>;
//            Assert.NotNull(result);
//            Assert.Empty(model);
//        }

//        // Test GET Create action
//        [Fact]
//        public void Create_ReturnsView()
//        {
//            // Act
//            var result = _controller.Create() as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//        }

//        // Test POST Create action with valid model
//        [Fact]
//        public async Task Create_ValidModel_RedirectsToIndex()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "New Note" };
//            _dbContextMock.Setup(db => db.Notes.Add(note));
//            //_dbContextMock.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

//            // Act
//            var result = await _controller.Create(note) as RedirectToActionResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Index", result.ActionName);
//        }

//        // Test POST Create action with invalid model
//        [Fact]
//        public async Task Create_InvalidModel_ReturnsViewWithErrorMessage()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "New Note" };
//            _controller.ModelState.AddModelError("NoteTitle", "Required");

//            // Act
//            var result = await _controller.Create(note) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.True(result.ViewData.ModelState.ContainsKey("NoteTitle"));
//        }

//        // Test POST Create action with exception
//        [Fact]
//        public async Task Create_Exception_ReturnsViewWithErrorMessage()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "New Note" };
//            //_dbContextMock.Setup(db => db.SaveChangesAsync()).ThrowsAsync(new System.Exception("Test Exception"));

//            // Act
//            var result = await _controller.Create(note) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Test Exception", _controller.TempData["errorMessage"]);
//        }

//        // Test GET Edit action with valid id
//        [Fact]
//        public async Task Edit_ExistingNote_ReturnsView()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "Edit Note" };
//            _dbContextMock.Setup(db => db.Notes.FindAsync(1)).ReturnsAsync(note);

//            // Act
//            var result = await _controller.Edit(1) as ViewResult;

//            // Assert
//            var model = result?.Model as NoteModel;
//            Assert.NotNull(result);
//            Assert.Equal("Edit Note", model?.NoteTitle);
//        }

//        // Test GET Edit action with invalid id
//        [Fact]
//        public async Task Edit_NoteNotFound_RedirectsToIndex()
//        {
//            // Arrange
//            _dbContextMock.Setup(db => db.Notes.FindAsync(It.IsAny<int>())).ReturnsAsync((NoteModel)null);

//            // Act
//            var result = await _controller.Edit(1) as RedirectToActionResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Index", result.ActionName);
//            Assert.Equal("note details not found with Id : 1", _controller.TempData["errorMessage"]);
//        }

//        // Test POST Edit action with valid model
//        [Fact]
//        public async Task Edit_ValidModel_UpdatesNote()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "Updated Note" };
//            var existingNote = new NoteModel { NoteId = 1, NoteTitle = "Old Note" };
//            _dbContextMock.Setup(db => db.Notes.FindAsync(1)).ReturnsAsync(existingNote);
//            //_dbContextMock.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

//            // Act
//            var result = await _controller.Edit(note) as RedirectToActionResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Index", result.ActionName);
//            Assert.Equal("note updated successfully.", _controller.TempData["successMessage"]);
//        }

//        // Test POST Edit action with invalid model
//        [Fact]
//        public async Task Edit_InvalidModel_ReturnsViewWithErrors()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "Updated Note" };
//            _controller.ModelState.AddModelError("NoteTitle", "Required");

//            // Act
//            var result = await _controller.Edit(note) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.True(result.ViewData.ModelState.ContainsKey("NoteTitle"));
//        }

//        // Test POST Edit action with note not found
//        [Fact]
//        public async Task Edit_NoteNotFound_ReturnsView()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "Updated Note" };
//            _dbContextMock.Setup(db => db.Notes.FindAsync(1)).ReturnsAsync((NoteModel)null);

//            // Act
//            var result = await _controller.Edit(note) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Note not found.", _controller.TempData["errorMessage"]);
//        }

//        // Test POST Delete action with existing note
//        [Fact]
//        public async Task Delete_NoteExists_DeletesNote()
//        {
//            // Arrange
//            var note = new NoteModel { NoteId = 1, NoteTitle = "Note to Delete" };
//            _dbContextMock.Setup(db => db.Notes.FindAsync(1)).ReturnsAsync(note);
//            _dbContextMock.Setup(db => db.Notes.Remove(note));
//            //_dbContextMock.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

//            // Act
//            var result = await _controller.Delete(1) as RedirectToActionResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Index", result.ActionName);
//            Assert.Equal("note deleted successfully.", _controller.TempData["successMessage"]);
//        }

//        // Test POST Delete action with note not found
//        [Fact]
//        public async Task Delete_NoteNotFound_ReturnsViewWithError()
//        {
//            // Arrange
//            _dbContextMock.Setup(db => db.Notes.FindAsync(1)).ReturnsAsync((NoteModel)null);

//            // Act
//            var result = await _controller.Delete(1) as ViewResult;

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Note not found with Id : 1", _controller.TempData["errorMessage"]);
//        }
//    }
//}
