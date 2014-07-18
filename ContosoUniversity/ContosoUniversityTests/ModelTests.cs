using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContosoUniversity.Models;

namespace ContosoUniversityTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestCourse()
        {
            Course c = new Course();
            Assert.AreEqual(0, c.Enrollments.Count);
        }
    }
}
