using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContosoUniversityTests
{
    [TestClass]
    public class ControllerCreateTests : DatabaseSetup
    {
        [TestMethod]
        public void ControllerCreate()
        {
            ConfirmDbSetup();
        }
    }
}
