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
                var node = new Node() { Key = "TheNode" };
                var leaf = new Leaf() { Key = "TheLeaf", Value = "TheValue" };

                var label = new Label() { Item = node, Content = "This is a Label" };
                var note = new Note() { Item = node, Content = "This is a Note" };

                node.Children.Add(leaf);
                leaf.Parent = node;

                node.Labels.Add(label);
                node.Notes.Add(note);

                db.Items.Add(node);
                db.Items.Add(leaf);
                db.Labels.Add(label);
                db.Notes.Add(note);

                db.SaveChanges();
            }
        }
    }
}
