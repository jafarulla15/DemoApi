USE [master]
GO
/****** Object:  Database [DemoProject]    Script Date: 8/7/2023 5:46:09 AM ******/
CREATE DATABASE [DemoProject]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DemoProject', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DemoProject.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DemoProject_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DemoProject_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DemoProject] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DemoProject].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DemoProject] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DemoProject] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DemoProject] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DemoProject] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DemoProject] SET ARITHABORT OFF 
GO
ALTER DATABASE [DemoProject] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DemoProject] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DemoProject] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DemoProject] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DemoProject] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DemoProject] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DemoProject] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DemoProject] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DemoProject] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DemoProject] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DemoProject] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DemoProject] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DemoProject] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DemoProject] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DemoProject] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DemoProject] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DemoProject] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DemoProject] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DemoProject] SET  MULTI_USER 
GO
ALTER DATABASE [DemoProject] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DemoProject] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DemoProject] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DemoProject] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DemoProject] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DemoProject] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [DemoProject] SET QUERY_STORE = OFF
GO
USE [DemoProject]
GO
/****** Object:  Table [dbo].[AccessToken]    Script Date: 8/7/2023 5:46:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessToken](
	[AccessTokenID] [int] IDENTITY(1,1) NOT NULL,
	[SystemUserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[IssuedOn] [datetime2](7) NOT NULL,
	[ExpiresOn] [datetime2](7) NOT NULL,
	[SessionId] [bigint] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_AccessToken] PRIMARY KEY CLUSTERED 
(
	[AccessTokenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Actions]    Script Date: 8/7/2023 5:46:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actions](
	[ActionID] [int] IDENTITY(1,1) NOT NULL,
	[ActionKey] [nvarchar](50) NOT NULL,
	[ActionName] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED 
(
	[ActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PagePermission]    Script Date: 8/7/2023 5:46:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PagePermission](
	[PagePermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PagePermissionName] [nvarchar](50) NOT NULL,
	[PageDisplayName] [nvarchar](50) NOT NULL,
	[Sequence] [int] NOT NULL,
 CONSTRAINT [PK_PagePermission] PRIMARY KEY CLUSTERED 
(
	[PagePermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleActionMapping]    Script Date: 8/7/2023 5:46:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleActionMapping](
	[RoleActionMappingID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[ActionID] [int] NOT NULL,
 CONSTRAINT [PK_RoleActionMapping] PRIMARY KEY CLUSTERED 
(
	[RoleActionMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePagePermissionMapping]    Script Date: 8/7/2023 5:46:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePagePermissionMapping](
	[RolePagePermissionMappingID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[PagePermissionID] [int] NOT NULL,
 CONSTRAINT [PK_RolePagePermissionMapping] PRIMARY KEY CLUSTERED 
(
	[RolePagePermissionMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/7/2023 5:46:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[RoleDetails] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemUser]    Script Date: 8/7/2023 5:46:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUser](
	[SystemUserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](150) NOT NULL,
	[RoleID] [int] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[StatusOfUser] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_SystemUser] PRIMARY KEY CLUSTERED 
(
	[SystemUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSession]    Script Date: 8/7/2023 5:46:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSession](
	[UserSessionID] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](max) NULL,
	[SystemUserID] [int] NULL,
	[SessionStart] [datetime2](7) NULL,
	[SessionEnd] [datetime2](7) NULL,
	[RoleId] [int] NULL,
	[Status] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_UserSession] PRIMARY KEY CLUSTERED 
(
	[UserSessionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SystemUser] ON 

INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (5, N'jafar', N'ulla', N'123456', N'jafar@inneed.cloud', N'$2a$11$fJfcZHNg43d83tMpRx3p9eoJgF6wbc9mdq.WxV.I1fI./lKTrIw8q', 0, 0, 1, CAST(N'2023-08-06T20:09:17.357' AS DateTime), 0, CAST(N'2023-08-06T20:09:17.357' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (6, N'jafar', N'ulla', N'123456', N'jafar2@inneed.cloud', N'$2a$11$2Bf6BpA.s3tgxG0t.BTwXeCeYD5yb380DHqRLjXIllLNmG16xJ6dC', 0, 0, 1, CAST(N'2023-08-06T20:27:17.780' AS DateTime), 0, CAST(N'2023-08-06T20:27:17.780' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (7, N'jafar', N'ulla', N'123456', N'jafar3@inneed.cloud', N'$2a$11$KRXU//eBbmwJJjeLHqrl5.ghTK1Cev6LaqsSp8FeJNgk0sjtOhKIG', 0, 0, 1, CAST(N'2023-08-06T20:27:53.590' AS DateTime), 0, CAST(N'2023-08-06T20:27:53.590' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (8, N'Test1', N'ulla', N'123456', N'test1@inneed.cloud', N'$2a$11$pVKcI.63c/TiEmmebjsBbuJ5JB1OX8u/iJ5Wm8JMgqo/vmSveH4DO', 0, 0, 1, CAST(N'2023-08-07T03:16:19.340' AS DateTime), 0, CAST(N'2023-08-07T03:16:19.340' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (9, N'jafar', N'ulla', N'123456', N'jafarulla14@gmail.com', N'$2a$11$KJ8Etnmgc48wdx5xHMsspOJMLeKk4.PEqX1nYgPcZw8mYeq8Z9qO2', 0, 0, 1, CAST(N'2023-08-07T04:22:15.070' AS DateTime), 0, CAST(N'2023-08-07T04:22:15.073' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (10, N'jafar', N'ulla', N'123456', N'jafarulla16@gmail.com', N'$2a$11$EZt0MsWvQnlEUKOD6V.Ke.uHdfKQgrfBEWeEyP7iQ7yMMSeA5PQJm', 0, 0, 1, CAST(N'2023-08-07T04:24:54.503' AS DateTime), 0, CAST(N'2023-08-07T04:24:54.503' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (11, N'jafar', N'ulla', N'123456', N'jafarulla15@gmail.com', N'$2a$11$P7FeRdjYUXtCaMENk0kcp.UC7kV/AJLndNiuz5hSNgZlfEZXssT8a', 0, 0, 1, CAST(N'2023-08-07T04:26:28.640' AS DateTime), 0, CAST(N'2023-08-07T04:26:28.640' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (12, N'jafar', N'ulla', N'123456', N'jafar123@inneed.cloud', N'$2a$11$MvAxxyldSlVnb.o.rv2g9.kRpK2qYQ3cV/pNpL.vI1Tbk/mkjchRO', 0, 0, 1, CAST(N'2023-08-07T05:19:03.427' AS DateTime), 0, CAST(N'2023-08-07T05:19:03.427' AS DateTime), 0, 1)
INSERT [dbo].[SystemUser] ([SystemUserID], [FirstName], [LastName], [PhoneNumber], [Email], [Password], [RoleID], [IsApproved], [StatusOfUser], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [Status]) VALUES (13, N'jafar', N'ulla', N'123456', N'test2@inneed.cloud', N'$2a$11$KUre195RHKIwsTlGBSaUpeznmyITZZWI8cwL2QB4BywzBoT5dMH0e', 0, 0, 1, CAST(N'2023-08-07T05:35:09.423' AS DateTime), 0, CAST(N'2023-08-07T05:35:09.423' AS DateTime), 0, 1)
SET IDENTITY_INSERT [dbo].[SystemUser] OFF
GO
SET IDENTITY_INSERT [dbo].[UserSession] ON 

INSERT [dbo].[UserSession] ([UserSessionID], [Token], [SystemUserID], [SessionStart], [SessionEnd], [RoleId], [Status], [CreatedBy], [CreatedDate]) VALUES (1, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTeXN0ZW1Vc2VySUQiOiI3IiwiVXNlclR5cGUiOiIwIiwiRXhwaXJlc09uIjoiOC84LzIwMjMgMTozMDo0NiBBTSIsIklzc3VlZE9uIjoiOC83LzIwMjMgMTozMDo0NiBBTSIsImp0aSI6IjMxZWY3NGJiLTE0MTYtNGI0NS1iNjEzLTJlYmU0NDJhOTYwOSIsImV4cCI6MTY5MTQzNjY0NiwiaXNzIjoiRGVtb1Byb2plY3QiLCJhdWQiOiJEZW1vUHJvamVjdCJ9.IDExaZfHcK_6936IcTbTu8IYbDYCF-Zgww0TJm3tM84', 7, CAST(N'2023-09-09T00:00:00.0000000' AS DateTime2), CAST(N'2023-08-07T03:51:07.2378244' AS DateTime2), 1, 2, 1, CAST(N'2023-09-09T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[UserSession] ([UserSessionID], [Token], [SystemUserID], [SessionStart], [SessionEnd], [RoleId], [Status], [CreatedBy], [CreatedDate]) VALUES (2, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTeXN0ZW1Vc2VySUQiOiI4IiwiVXNlclR5cGUiOiIwIiwiRXhwaXJlc09uIjoiOC84LzIwMjMgMzoxODozMiBBTSIsIklzc3VlZE9uIjoiOC83LzIwMjMgMzoxODozMiBBTSIsImp0aSI6ImJmNWMxMjYzLWEyNjctNGEwOS1hZDk5LTZiYTU4ODg1ZGJiYSIsImV4cCI6MTY5MTQ0MzExMiwiaXNzIjoiRGVtb1Byb2plY3QiLCJhdWQiOiJEZW1vUHJvamVjdCJ9.DgwC482c2R47BYVefPRyYhMFi5yCql-mq8FBLSvnJa8', 8, CAST(N'2023-08-07T03:18:33.1127439' AS DateTime2), CAST(N'2023-08-07T03:47:47.8690975' AS DateTime2), 0, 1, 0, NULL)
INSERT [dbo].[UserSession] ([UserSessionID], [Token], [SystemUserID], [SessionStart], [SessionEnd], [RoleId], [Status], [CreatedBy], [CreatedDate]) VALUES (3, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTeXN0ZW1Vc2VySUQiOiI3IiwiVXNlclR5cGUiOiIwIiwiRXhwaXJlc09uIjoiOC84LzIwMjMgMzo0MDo1NSBBTSIsIklzc3VlZE9uIjoiOC83LzIwMjMgMzo0MDo1NSBBTSIsImp0aSI6ImRiMGY3MzhjLWU0YzMtNGRmOS1iYjFiLWRiZjc1MzBlNmZmYSIsImV4cCI6MTY5MTQ0NDQ1NSwiaXNzIjoiRGVtb1Byb2plY3QiLCJhdWQiOiJEZW1vUHJvamVjdCJ9.kCajxXP8ihV35QLIU_Vd4VBD7s3HksaxKOa3Yh0_tu0', 7, CAST(N'2023-08-07T03:40:55.5645024' AS DateTime2), CAST(N'2023-08-07T03:51:07.2378306' AS DateTime2), 0, 2, 0, NULL)
INSERT [dbo].[UserSession] ([UserSessionID], [Token], [SystemUserID], [SessionStart], [SessionEnd], [RoleId], [Status], [CreatedBy], [CreatedDate]) VALUES (4, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTeXN0ZW1Vc2VySUQiOiI3IiwiVXNlclR5cGUiOiIwIiwiRXhwaXJlc09uIjoiOC84LzIwMjMgMzo1MDoxMSBBTSIsIklzc3VlZE9uIjoiOC83LzIwMjMgMzo1MDoxMSBBTSIsImp0aSI6IjBlYmU2MjQyLWI5NDctNGQ2ZS05MDU5LWJiODAzN2E5NWM1YyIsImV4cCI6MTY5MTQ0NTAxMSwiaXNzIjoiRGVtb1Byb2plY3QiLCJhdWQiOiJEZW1vUHJvamVjdCJ9.7hbVRX_fjaqjuFnLg7W9sxuK7afdfz6jN4u2Dotz7e0', 7, CAST(N'2023-08-07T03:50:11.7548282' AS DateTime2), CAST(N'2023-08-07T03:51:07.2378309' AS DateTime2), 0, 2, 0, NULL)
INSERT [dbo].[UserSession] ([UserSessionID], [Token], [SystemUserID], [SessionStart], [SessionEnd], [RoleId], [Status], [CreatedBy], [CreatedDate]) VALUES (5, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTeXN0ZW1Vc2VySUQiOiIxMyIsIlVzZXJUeXBlIjoiMCIsIkV4cGlyZXNPbiI6IjgvOC8yMDIzIDU6MzU6NTUgQU0iLCJJc3N1ZWRPbiI6IjgvNy8yMDIzIDU6MzU6NTUgQU0iLCJqdGkiOiJhMzM3Nzk3NS1jNDg5LTRhNWMtODNmOC00OGEwNDZmMzU3NjkiLCJleHAiOjE2OTE0NTEzNTUsImlzcyI6IkRlbW9Qcm9qZWN0IiwiYXVkIjoiRGVtb1Byb2plY3QifQ.A08TZQC_hwJdUSxpKDf74oRZbSAKbR1mpyNRO-xCVag', 13, CAST(N'2023-08-07T05:35:55.9286503' AS DateTime2), CAST(N'2023-08-07T05:36:48.8669581' AS DateTime2), 0, 2, 0, NULL)
SET IDENTITY_INSERT [dbo].[UserSession] OFF
GO
ALTER TABLE [dbo].[AccessToken] ADD  CONSTRAINT [DF_AccessToken_Status]  DEFAULT ((0)) FOR [Status]
GO
USE [master]
GO
ALTER DATABASE [DemoProject] SET  READ_WRITE 
GO
