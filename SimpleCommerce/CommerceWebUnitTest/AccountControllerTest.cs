using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Ploeh.Samples.Commerce.Web.Controllers;
using Xunit;

namespace Ploeh.Samples.Commerce.WebUnitTest
{
    public class AccountControllerTest
    {
        [Fact]
        public void ChangePasswordGetReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword();

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
        }

        [Fact]
        public void ChangePasswordPostRedirectsOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.ChangePassword("oldPass", "newPass", "newPass");

            // Assert
            Assert.Equal("ChangePasswordSuccess", result.RouteValues["action"]);
        }

        [Fact]
        public void ChangePasswordPostReturnsViewIfCurrentPasswordNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("", "newPassword", "newPassword");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("You must specify a current password.", result.ViewData.ModelState["currentPassword"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void ChangePasswordPostReturnsViewIfNewPasswordDoesNotMatchConfirmPassword()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("currentPassword", "newPassword", "otherPassword");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("The new password and confirmation password do not match.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void ChangePasswordPostReturnsViewIfNewPasswordIsNull()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("currentPassword", null, null);

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("You must specify a new password of 6 or more characters.", result.ViewData.ModelState["newPassword"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void ChangePasswordPostReturnsViewIfNewPasswordIsTooShort()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("currentPassword", "12345", "12345");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("You must specify a new password of 6 or more characters.", result.ViewData.ModelState["newPassword"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void ChangePasswordPostReturnsViewIfProviderRejectsPassword()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePassword("oldPass", "badPass", "badPass");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("The current password is incorrect or the new password is invalid.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void ChangePasswordSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.ChangePasswordSuccess();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ConstructorSetsProperties()
        {
            // Arrange
            IFormsAuthentication formsAuth = new MockFormsAuthenticationService();
            IMembershipService membershipService = new AccountMembershipService();

            // Act
            AccountController controller = new AccountController(formsAuth, membershipService);

            // Assert
            Assert.Equal(formsAuth, controller.FormsAuth);
            Assert.Equal(membershipService, controller.MembershipService);
        }

        [Fact]
        public void ConstructorSetsPropertiesToDefaultValues()
        {
            // Act
            AccountController controller = new AccountController();

            // Assert
            Assert.NotNull(controller.FormsAuth);
            Assert.NotNull(controller.MembershipService);
        }

        [Fact]
        public void LoginGet()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LoginPostRedirectsHomeIfLoginSuccessfulButNoReturnUrlGiven()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOn("someUser", "goodPass", true, null);

            // Assert
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        public void LoginPostRedirectsToReturnUrlIfLoginSuccessfulAndReturnUrlGiven()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectResult result = (RedirectResult)controller.LogOn("someUser", "goodPass", false, "someUrl");

            // Assert
            Assert.Equal("someUrl", result.Url);
        }

        [Fact]
        public void LoginPostReturnsViewIfPasswordNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn("username", "", true, null);

            // Assert
            Assert.Equal("You must specify a password.", result.ViewData.ModelState["password"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void LoginPostReturnsViewIfUsernameNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn("", "somePass", false, null);

            // Assert
            Assert.Equal("You must specify a username.", result.ViewData.ModelState["username"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void LoginPostReturnsViewIfUsernameOrPasswordIsIncorrect()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.LogOn("someUser", "badPass", true, null);

            // Assert
            Assert.Equal("The username or password provided is incorrect.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void LogOff()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.LogOff();

            // Assert
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        public void RegisterGet()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register();

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
        }

        [Fact]
        public void RegisterPostRedirectsHomeIfRegistrationSuccessful()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Register("someUser", "email", "goodPass", "goodPass");

            // Assert
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        public void RegisterPostReturnsViewIfEmailNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "", "password", "password");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("You must specify an email address.", result.ViewData.ModelState["email"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void RegisterPostReturnsViewIfNewPasswordDoesNotMatchConfirmPassword()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "email", "password", "password2");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("The new password and confirmation password do not match.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void RegisterPostReturnsViewIfPasswordIsNull()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "email", null, null);

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("You must specify a password of 6 or more characters.", result.ViewData.ModelState["password"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void RegisterPostReturnsViewIfPasswordIsTooShort()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("username", "email", "12345", "12345");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("You must specify a password of 6 or more characters.", result.ViewData.ModelState["password"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void RegisterPostReturnsViewIfRegistrationFails()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("someUser", "DuplicateUserName" /* error */, "badPass", "badPass");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("Username already exists. Please enter a different user name.", result.ViewData.ModelState["_FORM"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void RegisterPostReturnsViewIfUsernameNotSpecified()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ViewResult result = (ViewResult)controller.Register("", "email", "password", "password");

            // Assert
            Assert.Equal(6, result.ViewData["PasswordLength"]);
            Assert.Equal("You must specify a username.", result.ViewData.ModelState["username"].Errors[0].ErrorMessage);
        }

        private static AccountController GetAccountController()
        {
            IFormsAuthentication formsAuth = new MockFormsAuthenticationService();
            MembershipProvider membershipProvider = new MockMembershipProvider();
            AccountMembershipService membershipService = new AccountMembershipService(membershipProvider);
            AccountController controller = new AccountController(formsAuth, membershipService);
            ControllerContext controllerContext = new ControllerContext(new MockHttpContext(), new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            return controller;
        }

        public class MockFormsAuthenticationService : IFormsAuthentication
        {
            public void SignIn(string userName, bool createPersistentCookie)
            {
            }

            public void SignOut()
            {
            }
        }

        public class MockIdentity : IIdentity
        {
            public string AuthenticationType
            {
                get
                {
                    return "MockAuthentication";
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return true;
                }
            }

            public string Name
            {
                get
                {
                    return "someUser";
                }
            }
        }

        public class MockPrincipal : IPrincipal
        {
            IIdentity _identity;

            public IIdentity Identity
            {
                get
                {
                    if (_identity == null)
                    {
                        _identity = new MockIdentity();
                    }
                    return _identity;
                }
            }

            public bool IsInRole(string role)
            {
                return false;
            }
        }

        public class MockMembershipUser : MembershipUser
        {
            public override bool ChangePassword(string oldPassword, string newPassword)
            {
                return newPassword.Equals("newPass");
            }
        }

        public class MockHttpContext : HttpContextBase
        {
            private IPrincipal _user;

            public override IPrincipal User
            {
                get
                {
                    if (_user == null)
                    {
                        _user = new MockPrincipal();
                    }
                    return _user;
                }
                set
                {
                    _user = value;
                }
            }
        }

        public class MockMembershipProvider : MembershipProvider
        {
            string _applicationName;

            public override string ApplicationName
            {
                get
                {
                    return _applicationName;
                }
                set
                {
                    _applicationName = value;
                }
            }

            public override bool EnablePasswordReset
            {
                get
                {
                    return false;
                }
            }

            public override bool EnablePasswordRetrieval
            {
                get
                {
                    return false;
                }
            }

            public override int MaxInvalidPasswordAttempts
            {
                get
                {
                    return 0;
                }
            }

            public override int MinRequiredNonAlphanumericCharacters
            {
                get
                {
                    return 0;
                }
            }

            public override int MinRequiredPasswordLength
            {
                get
                {
                    return 6;
                }
            }

            public override string Name
            {
                get
                {
                    return null;
                }
            }

            public override int PasswordAttemptWindow
            {
                get
                {
                    return 3;
                }
            }

            public override MembershipPasswordFormat PasswordFormat
            {
                get
                {
                    return MembershipPasswordFormat.Clear;
                }
            }

            public override string PasswordStrengthRegularExpression
            {
                get
                {
                    return null;
                }
            }

            public override bool RequiresQuestionAndAnswer
            {
                get
                {
                    return false;
                }
            }

            public override bool RequiresUniqueEmail
            {
                get
                {
                    return false;
                }
            }

            public override bool ChangePassword(string username, string oldPassword, string newPassword)
            {
                throw new NotImplementedException();
            }

            public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
            {
                throw new NotImplementedException();
            }

            public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, Object providerUserKey, out MembershipCreateStatus status)
            {
                MockMembershipUser user = new MockMembershipUser();

                if (username.Equals("someUser") && password.Equals("goodPass") && email.Equals("email"))
                {
                    status = MembershipCreateStatus.Success;
                }
                else
                {
                    // the 'email' parameter contains the status we want to return to the user
                    status = (MembershipCreateStatus)Enum.Parse(typeof(MembershipCreateStatus), email);
                }

                return user;
            }

            public override bool DeleteUser(string username, bool deleteAllRelatedData)
            {
                throw new NotImplementedException();
            }

            public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
            {
                throw new NotImplementedException();
            }

            public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
            {
                throw new NotImplementedException();
            }

            public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
            {
                throw new NotImplementedException();
            }

            public override int GetNumberOfUsersOnline()
            {
                throw new NotImplementedException();
            }

            public override string GetPassword(string username, string answer)
            {
                throw new NotImplementedException();
            }

            public override string GetUserNameByEmail(string email)
            {
                throw new NotImplementedException();
            }

            public override MembershipUser GetUser(Object providerUserKey, bool userIsOnline)
            {
                throw new NotImplementedException();
            }

            public override MembershipUser GetUser(string username, bool userIsOnline)
            {
                return new MockMembershipUser();
            }

            public override string ResetPassword(string username, string answer)
            {
                throw new NotImplementedException();
            }

            public override bool UnlockUser(string userName)
            {
                throw new NotImplementedException();
            }

            public override void UpdateUser(MembershipUser user)
            {
                throw new NotImplementedException();
            }

            public override bool ValidateUser(string username, string password)
            {
                return password.Equals("goodPass");
            }

        }
    }
}
