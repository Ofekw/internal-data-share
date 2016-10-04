USE [coevolutions]
GO
SET IDENTITY_INSERT [dbo].[Items] ON 

GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (1, N'KiwiBank', 0, CAST(0x0000A69500F3B920 AS DateTime), CAST(0x0000A69500F3B920 AS DateTime), NULL, N'Node', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (2, N'ASB', 0, CAST(0x0000A69500F3B920 AS DateTime), CAST(0x0000A69500F3B920 AS DateTime), NULL, N'Node', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (3, N'BNZ', 0, CAST(0x0000A69500F3B920 AS DateTime), CAST(0x0000A69500F3B920 AS DateTime), NULL, N'Node', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (4, N'Email', 0, CAST(0x0000A69500F3B938 AS DateTime), CAST(0x0000A69500F3B938 AS DateTime), N'example@testEmail.com', N'Leaf', 1, 1, NULL, 0)
GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (5, N'Password', 0, CAST(0x0000A69500F3B938 AS DateTime), CAST(0x0000A69500F3B938 AS DateTime), N'ExamplePassword', N'Leaf', 1, 1, NULL, 0)
GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (6, N'VirtualMachine', 0, CAST(0x0000A69500F3B93D AS DateTime), CAST(0x0000A69500F3B93D AS DateTime), NULL, N'Node', 1, 1, NULL, NULL)
GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (7, N'Login', 0, CAST(0x0000A69500F3B93F AS DateTime), CAST(0x0000A69500F3B93F AS DateTime), N'Admin', N'Leaf', 6, 6, NULL, 0)
GO
INSERT [dbo].[Items] ([Id], [Key], [Deleted], [CreatedOn], [UpdatedOn], [Value], [Discriminator], [Parent_Id], [Node_Id], [Note], [Stale]) VALUES (8, N'Password', 0, CAST(0x0000A69500F3B93F AS DateTime), CAST(0x0000A69500F3B93F AS DateTime), N'ExamplePassword', N'Leaf', 6, 6, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[Items] OFF
GO
SET IDENTITY_INSERT [dbo].[Notes] ON 

GO
INSERT [dbo].[Notes] ([Id], [Content], [CreatedOn], [UpdatedOn], [Item_Id]) VALUES (1, N'CreatedBySeedMethod', CAST(0x0000A69500F3B920 AS DateTime), CAST(0x0000A69500F3B920 AS DateTime), 3)
GO
INSERT [dbo].[Notes] ([Id], [Content], [CreatedOn], [UpdatedOn], [Item_Id]) VALUES (2, N'CreatedBySeedMethod', CAST(0x0000A69500F3B938 AS DateTime), CAST(0x0000A69500F3B938 AS DateTime), 4)
GO
INSERT [dbo].[Notes] ([Id], [Content], [CreatedOn], [UpdatedOn], [Item_Id]) VALUES (3, N'CreatedBySeedMethod', CAST(0x0000A69500F3B938 AS DateTime), CAST(0x0000A69500F3B938 AS DateTime), 5)
GO
INSERT [dbo].[Notes] ([Id], [Content], [CreatedOn], [UpdatedOn], [Item_Id]) VALUES (4, N'CreatedBySeedMethod', CAST(0x0000A69500F3B93D AS DateTime), CAST(0x0000A69500F3B93D AS DateTime), 6)
GO
INSERT [dbo].[Notes] ([Id], [Content], [CreatedOn], [UpdatedOn], [Item_Id]) VALUES (5, N'CreatedBySeedMethod', CAST(0x0000A69500F3B93F AS DateTime), CAST(0x0000A69500F3B93F AS DateTime), 7)
GO
INSERT [dbo].[Notes] ([Id], [Content], [CreatedOn], [UpdatedOn], [Item_Id]) VALUES (6, N'CreatedBySeedMethod', CAST(0x0000A69500F3B93F AS DateTime), CAST(0x0000A69500F3B93F AS DateTime), 8)
GO
SET IDENTITY_INSERT [dbo].[Notes] OFF
GO
SET IDENTITY_INSERT [dbo].[Labels] ON 

GO
INSERT [dbo].[Labels] ([Id], [Content]) VALUES (1, N'Bank')
GO
INSERT [dbo].[Labels] ([Id], [Content]) VALUES (2, N'Virtual Machine')
GO
INSERT [dbo].[Labels] ([Id], [Content]) VALUES (3, N'Password')
GO
INSERT [dbo].[Labels] ([Id], [Content]) VALUES (4, N'Out of date')
GO
INSERT [dbo].[Labels] ([Id], [Content]) VALUES (5, N'Test Machine')
GO
INSERT [dbo].[Labels] ([Id], [Content]) VALUES (6, N'External')
GO
SET IDENTITY_INSERT [dbo].[Labels] OFF
GO
INSERT [dbo].[LabelItems] ([Label_Id], [Item_Id]) VALUES (1, 1)
GO
INSERT [dbo].[LabelItems] ([Label_Id], [Item_Id]) VALUES (1, 2)
GO
INSERT [dbo].[LabelItems] ([Label_Id], [Item_Id]) VALUES (1, 3)
GO
INSERT [dbo].[LabelItems] ([Label_Id], [Item_Id]) VALUES (3, 4)
GO
INSERT [dbo].[LabelItems] ([Label_Id], [Item_Id]) VALUES (4, 5)
GO
INSERT [dbo].[LabelItems] ([Label_Id], [Item_Id]) VALUES (2, 6)
GO