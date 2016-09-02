using System.Collections.Generic;
using Coevolution.Models;

namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Coevolution.Models.ModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Coevolution.Models.ModelContext context)
        {
            context.Labels.AddOrUpdate(
                x => x.Id,
                new Label()
                {
                    Id = 1,
                    Content = "Bank"
                },
                new Label()
                {
                    Id = 2,
                    Content = "Virtual Machine"
                },
                new Label()
                {
                    Id = 3,
                    Content = "Password"
                },
                new Label()
                {
                    Id = 4,
                    Content = "Out of date"
                },
                new Label()
                {
                    Id = 5,
                    Content = "Test Machine"
                },
                new Label()
                {
                    Id = 6,
                    Content = "External"
                }
            );
            context.SaveChanges();
            var bankLabel = context.Labels.First(x => x.Content.Equals("Bank"));
            var passwordLabel = context.Labels.First(x=>x.Content.Equals("Password"));
            var outOfDateLabel = context.Labels.First(x=>x.Content.Equals("Out of Date"));
            var vmLabel = context.Labels.First(x=>x.Content.Equals("Virtual Machine"));

            context.Items.AddOrUpdate(
                n => n.Id,
                new Node()
                {
                    Id = 1,
                    Deleted = false,
                    Labels = new List<Label>() { bankLabel },
                    Parent = null,
                    Key = "KiwiBank",
                    Notes = null,
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now
                },
                new Node()
                {
                    Id = 2,
                    Deleted = false,
                    Labels = new List<Label>() { bankLabel },
                    Parent = null,
                    Key = "ASB",
                    Notes = null,
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now
                },
                new Node()
                {
                    Id = 3,
                    Deleted = false,
                    Labels = new List<Label>() { bankLabel },
                    Parent = null,
                    Key = "BNZ",
                    Notes = new List<Note>() {new Note() {Content = "CreatedBySeedMethod", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now} },
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now
                }
            );
            context.SaveChanges();

            context.Items.AddOrUpdate(x => x.Id,
                new Leaf()
                {
                    Id = 4,
                    Deleted = false,
                    Labels = new List<Label>() {passwordLabel},
                    Parent = context.Items.Find(1),
                    Key = "Email",
                    Notes =
                        new List<Note>()
                        {
                            new Note()
                            {
                                Content = "CreatedBySeedMethod",
                                CreatedOn = DateTime.Now,
                                UpdatedOn = DateTime.Now
                            }
                        },
                    Value = "example@testEmail.com",
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now
                },
                new Leaf()
                {
                    Id = 5,
                    Deleted = false,
                    Labels = new List<Label>() {outOfDateLabel},
                    Parent = context.Items.Find(1),
                    Key = "Password",
                    Notes =
                        new List<Note>()
                        {
                            new Note()
                            {
                                Content = "CreatedBySeedMethod",
                                CreatedOn = DateTime.Now,
                                UpdatedOn = DateTime.Now
                            }
                        },
                    Value = "ExamplePassword",
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now
                });
            context.SaveChanges();
            context.Items.AddOrUpdate(x => x.Id,
                new Node()
                  {
                      Id = 6,
                      Deleted = false,
                      Labels = new List<Label>() { vmLabel },
                      Parent = context.Items.Find(1),
                      Key = "VirtualMachine",
                      Notes =
                        new List<Note>()
                        {
                            new Note() {Content = "CreatedBySeedMethod", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now}
                        },
                      UpdatedOn = DateTime.Now,
                      CreatedOn = DateTime.Now
                  }
            );
            context.SaveChanges();
            context.Items.AddOrUpdate(x => x.Id,
                new Leaf()
                {
                    Id = 7,
                    Deleted = false,
                    Labels = new List<Label>() {},
                    Parent = context.Items.Find(6),
                    Key = "Login",
                    Notes =
                        new List<Note>()
                        {
                            new Note()
                            {
                                Content = "CreatedBySeedMethod",
                                CreatedOn = DateTime.Now,
                                UpdatedOn = DateTime.Now
                            }
                        },
                    Value = "Admin",
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now
                },
                new Leaf()
                {
                    Id = 8,
                    Deleted = false,
                    Labels = new List<Label>() {},
                    Parent = context.Items.Find(6),
                    Key = "Password",
                    Notes =
                        new List<Note>()
                        {
                            new Note()
                            {
                                Content = "CreatedBySeedMethod",
                                CreatedOn = DateTime.Now,
                                UpdatedOn = DateTime.Now
                            }
                        },
                    Value = "ExamplePassword",
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now
                }
            );
        }
    }
}
