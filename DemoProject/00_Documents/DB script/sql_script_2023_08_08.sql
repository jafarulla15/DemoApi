USE [master]
GO
/****** Object:  Database [DemoProject]    Script Date: 8/8/2023 1:38:24 AM ******/
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
/****** Object:  Table [dbo].[Actions]    Script Date: 8/8/2023 1:38:24 AM ******/
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
/****** Object:  Table [dbo].[AuditLog]    Script Date: 8/8/2023 1:38:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLog](
	[LogID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[FormName] [nvarchar](50) NOT NULL,
	[CalledFunction] [nvarchar](50) NOT NULL,
	[ActionID] [int] NOT NULL,
	[UserRightID] [int] NOT NULL,
	[UserTypeID] [int] NOT NULL,
	[LogMessage] [nvarchar](250) NOT NULL,
	[LogRefMessage] [nvarchar](250) NOT NULL,
	[IsObj] [bit] NOT NULL,
	[CompanyID] [int] NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[LogTypeID] [int] NOT NULL,
	[SessionID] [bigint] NOT NULL,
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExceptionLog]    Script Date: 8/8/2023 1:38:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionLog](
	[ExceptionLogID] [int] IDENTITY(1,1) NOT NULL,
	[Priority] [int] NOT NULL,
	[ModuleID] [int] NOT NULL,
	[ExceptionMessege] [nvarchar](500) NOT NULL,
	[ExceptionDetail] [nvarchar](1050) NOT NULL,
	[ObjectData] [nvarchar](500) NOT NULL,
	[ControllerName] [nvarchar](50) NOT NULL,
	[ActionName] [nvarchar](50) NOT NULL,
	[ActionType] [int] NOT NULL,
	[ManagerName] [nvarchar](50) NOT NULL,
	[ExceptionTime] [datetime] NOT NULL,
	[FixStatus] [int] NOT NULL,
 CONSTRAINT [PK_ExceptionLog] PRIMARY KEY CLUSTERED 
(
	[ExceptionLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PagePermission]    Script Date: 8/8/2023 1:38:24 AM ******/
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
/****** Object:  Table [dbo].[RoleActionMapping]    Script Date: 8/8/2023 1:38:24 AM ******/
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
/****** Object:  Table [dbo].[RolePagePermissionMapping]    Script Date: 8/8/2023 1:38:24 AM ******/
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
/****** Object:  Table [dbo].[Roles]    Script Date: 8/8/2023 1:38:24 AM ******/
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
/****** Object:  Table [dbo].[SystemUser]    Script Date: 8/8/2023 1:38:24 AM ******/
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
/****** Object:  Table [dbo].[UserSession]    Script Date: 8/8/2023 1:38:24 AM ******/
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
USE [master]
GO
ALTER DATABASE [DemoProject] SET  READ_WRITE 
GO
