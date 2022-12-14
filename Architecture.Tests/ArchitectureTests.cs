using NetArchTest.Rules;

namespace Architecture.Tests
{
    public class ArchitectureTests
    {
        private const string DomainNameSpace = "Domain";
        private const string ApplicationNameSpace = "Application";
        private const string InfrastructureNameSpace = "Infrastructure";
        private const string WebApiNameSpace = "WebApi";

        [Fact]
        public void Domain_Should_Not_haveDependencyOnOtherProjects()
        {
            //Arrange
            var assembly = typeof(Domain.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                ApplicationNameSpace,
                InfrastructureNameSpace,
                WebApiNameSpace
            };

            //Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            //Assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Application_Should_Not_haveDependencyOnOtherProjects()
        {
            //Arrange
            var assembly = typeof(Application.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                InfrastructureNameSpace,
                WebApiNameSpace
            };

            //Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            //Assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Infrastructure_Should_Not_haveDependencyOnOtherProjects()
        {
            //Arrange
            var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                WebApiNameSpace
            };

            //Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            //Assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Handlers_Should_Have_DependencyOnDomain()
        {
            //Arrange
            var assembly = typeof(Application.AssemblyReference).Assembly;

            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handlers")
                .Should()
                .HaveDependencyOn(DomainNameSpace)
                .GetResult();

            //Assert
            Assert.True(testResult.IsSuccessful);

        }

        [Fact]
        public void Controllers_Should_haveDependencyOnMediatR()
        {
            //Arrange
            var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            //Assert
            Assert.True(testResult.IsSuccessful);
        }
    }
}
