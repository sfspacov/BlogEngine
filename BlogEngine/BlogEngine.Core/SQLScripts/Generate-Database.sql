DROP DATABASE IF EXISTS BlogEngine
CREATE DATABASE BlogEngine
GO

USE [BlogEngine]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 09/02/2023 00:46:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [int] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerReviews]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerReviews](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LastUpdateDate] [datetime2](7) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[CustomerName] [nvarchar](max) NULL,
	[CustomerEmail] [nvarchar](max) NOT NULL,
	[Rate] [int] NOT NULL,
	[Message] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_CustomerReviews] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LastUpdateDate] [datetime2](7) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[Body] [nvarchar](300) NOT NULL,
	[ApplicationUserID] [int] NOT NULL,
	[PostID] [int] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationReceivers]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationReceivers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LastUpdateDate] [datetime2](7) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[EmailAddress] [nvarchar](max) NOT NULL,
	[DisplayName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_NotificationReceivers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostRatings]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostRatings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LastUpdateDate] [datetime2](7) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[Rate] [int] NOT NULL,
	[PostID] [int] NOT NULL,
	[ApplicationUserID] [int] NOT NULL,
 CONSTRAINT [PK_PostRatings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LastUpdateDate] [datetime2](7) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[ApplicationUserID] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[EditorsReview] [nvarchar](max) NULL,
	[EstimatedReadingTimeInMinutes] [int] NOT NULL,
	[Published] [datetime2](7) NOT NULL,
	[PostStatusID] [int] NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostStatus]    Script Date: 09/02/2023 00:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LastUpdateDate] [datetime2](7) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_PostStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (N'') FOR [FirstName]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (N'') FOR [LastName]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT ((0)) FOR [ApplicationUserID]
GO
ALTER TABLE [dbo].[Posts] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Published]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Posts_PostID] FOREIGN KEY([PostID])
REFERENCES [dbo].[Posts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Posts_PostID]
GO
ALTER TABLE [dbo].[PostRatings]  WITH CHECK ADD  CONSTRAINT [FK_PostRatings_Posts_PostID] FOREIGN KEY([PostID])
REFERENCES [dbo].[Posts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostRatings] CHECK CONSTRAINT [FK_PostRatings_Posts_PostID]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_AspNetUsers_ApplicationUserID] FOREIGN KEY([ApplicationUserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_AspNetUsers_ApplicationUserID]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_PostStatus_PostStatusID] FOREIGN KEY([PostStatusID])
REFERENCES [dbo].[PostStatus] ([ID])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_PostStatus_PostStatusID]
GO

SET IDENTITY_INSERT AspNetUsers ON

INSERT INTO AspNetUsers ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName])
                 VALUES 
				 (1, N'shakespeare@email.uk', N'SHAKESPEARE@EMAIL.UK', N'shakespeare@email.uk', N'SHAKESPEARE@EMAIL.UK', 0, N'AQAAAAEAACcQAAAAEDRKJzjUQt6Uc4dZdMEGTHFXcI9I2cOckUqJEos0+taZC/TTxEaBnk3cYTGGYstgGA==', N'BHW5LWMPRSZVFFK4AI4BXU4Y4KW55EIP', N'4b8330c7-e105-41df-8090-1a6ff9467074', NULL, 0, 0, NULL, 1, 0, N'William', N'Shakespeare'),
				 (2, N'dostoievski@email.ru', N'DOSTOIEVSKI@EMAIL.RU', N'dostoievski@email.ru', N'DOSTOIEVSKI@EMAIL.RU', 0, N'AQAAAAEAACcQAAAAEDRKJzjUQt6Uc4dZdMEGTHFXcI9I2cOckUqJEos0+taZC/TTxEaBnk3cYTGGYstgGA==', N'BHW5LWMPRSZVFFK4AI4BXU4Y4KW55EIP', N'4b8330c7-e105-41df-8090-1a6ff9467074', NULL, 0, 0, NULL, 1, 0, N'Fiodor', N'Dostoievski'),
				 (3, N'editor@email.com', N'EDITOR@EMAIL.COM', N'editor@email.com', N'EDITOR@EMAIL.COM', 0, N'AQAAAAEAACcQAAAAEDRKJzjUQt6Uc4dZdMEGTHFXcI9I2cOckUqJEos0+taZC/TTxEaBnk3cYTGGYstgGA==', N'BHW5LWMPRSZVFFK4AI4BXU4Y4KW55EIP', N'4b8330c7-e105-41df-8090-1a6ff9467074', NULL, 0, 0, NULL, 1, 0, N'Editor', N'Santos'),
				 (4, N'public@email.com', N'PUBLIC@EMAIL.COM', N'public@email.com', N'PUBLIC@EMAIL.COM', 0, N'AQAAAAEAACcQAAAAEDRKJzjUQt6Uc4dZdMEGTHFXcI9I2cOckUqJEos0+taZC/TTxEaBnk3cYTGGYstgGA==', N'BHW5LWMPRSZVFFK4AI4BXU4Y4KW55EIP', N'4b8330c7-e105-41df-8090-1a6ff9467074', NULL, 0, 0, NULL, 1, 0, N'Public', N'Enemy'),
				 (5, N'admin@email.com', N'ADMIN@EMAIL.COM', N'admin@email.com', N'ADMIN@EMAIL.COM', 0, N'AQAAAAEAACcQAAAAEDRKJzjUQt6Uc4dZdMEGTHFXcI9I2cOckUqJEos0+taZC/TTxEaBnk3cYTGGYstgGA==', N'BHW5LWMPRSZVFFK4AI4BXU4Y4KW55EIP', N'4b8330c7-e105-41df-8090-1a6ff9467074', NULL, 0, 0, NULL, 1, 0, N'Super', N'Admin')

SET IDENTITY_INSERT AspNetUsers OFF

SET IDENTITY_INSERT AspNetUserClaims ON

INSERT INTO AspNetUserClaims ([Id], [UserId], [ClaimType], [ClaimValue])
VALUES 
	(1, 1, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Writer'),
	(2, 2, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Writer'),
	(3, 3, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Editor'),
	(4, 4, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Public'),
	(5, 5, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Admin')

SET IDENTITY_INSERT AspNetUserClaims OFF

SET IDENTITY_INSERT PostStatus ON

INSERT INTO PostStatus (Id, [Description])
                  VALUES(1, 'PendingApproval'),(2, 'Approved'),(3, 'Rejected')

SET IDENTITY_INSERT PostStatus OFF