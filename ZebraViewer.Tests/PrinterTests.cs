using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZebraViewer.Services;

namespace ZebraViewer.Tests
{
    [TestClass]
    public class PrinterTests
    {
        [TestMethod]
        public void HasWorkingOperationalSystemName()
        {
            Assert.AreEqual(SystemUtilities.GetOperationalSystemName(), "Microsoft Windows 10 Pro");
        }

        [TestMethod]
        public void PrinterListIsNotNull()
        {
            Assert.IsNotNull(PrinterService.GetPrinters());
        }
    }
}
