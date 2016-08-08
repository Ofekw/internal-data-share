using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coevolution.Models;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new ModelContext())
            {
                var node = new Node();
                var leaf = new Leaf();
                node.Children.Add(leaf);
                node.Labels.Add("This is a Label");
                node.Notes.Add("This is a Note");
                db.Items.Add(node);
                db.Items.Add(leaf);
                db.SaveChanges();
            }
        }
    }
}
