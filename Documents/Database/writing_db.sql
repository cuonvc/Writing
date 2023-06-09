USE [master]
GO
/****** Object:  Database [Writing]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE DATABASE [Writing]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Writing', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Writing.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Writing_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Writing_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Writing] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Writing].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Writing] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Writing] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Writing] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Writing] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Writing] SET ARITHABORT OFF 
GO
ALTER DATABASE [Writing] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Writing] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Writing] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Writing] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Writing] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Writing] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Writing] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Writing] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Writing] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Writing] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Writing] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Writing] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Writing] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Writing] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Writing] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Writing] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Writing] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Writing] SET RECOVERY FULL 
GO
ALTER DATABASE [Writing] SET  MULTI_USER 
GO
ALTER DATABASE [Writing] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Writing] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Writing] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Writing] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Writing] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Writing] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Writing', N'ON'
GO
ALTER DATABASE [Writing] SET QUERY_STORE = ON
GO
ALTER DATABASE [Writing] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Writing]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories_table]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories_table](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Categories_table] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryPost]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryPost](
	[CategoriesId] [int] NOT NULL,
	[PostsId] [int] NOT NULL,
 CONSTRAINT [PK_CategoryPost] PRIMARY KEY CLUSTERED 
(
	[CategoriesId] ASC,
	[PostsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments_tbl]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments_tbl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[content] [nvarchar](max) NOT NULL,
	[PostId] [int] NOT NULL,
	[UserId] [int] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Comments_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts_tbl]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts_tbl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Thumbnail] [nvarchar](max) NULL,
	[Vote] [int] NOT NULL,
	[View] [int] NOT NULL,
	[Pined] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[IsPending] [bit] NOT NULL,
 CONSTRAINT [PK_Posts_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens_tbl]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens_tbl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[ExpireDate] [datetime2](7) NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_RefreshTokens_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Relationships_tbl]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Relationships_tbl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FollowerId] [int] NULL,
	[FollowingId] [int] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Relationships_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPostVote_tbl]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPostVote_tbl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[PostId] [int] NULL,
	[VoteType] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserPostVote_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users_tbl]    Script Date: 6/17/2023 9:24:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users_tbl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](450) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Salt] [varbinary](max) NOT NULL,
	[About] [nvarchar](max) NULL,
	[Gender] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[AvatarPhoto] [nvarchar](max) NULL,
	[CoverPhoto] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users_tbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Categories_table_Name]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Categories_table_Name] ON [dbo].[Categories_table]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CategoryPost_PostsId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_CategoryPost_PostsId] ON [dbo].[CategoryPost]
(
	[PostsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_tbl_PostId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_tbl_PostId] ON [dbo].[Comments_tbl]
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_tbl_UserId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_tbl_UserId] ON [dbo].[Comments_tbl]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Posts_tbl_UserId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_Posts_tbl_UserId] ON [dbo].[Posts_tbl]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RefreshTokens_tbl_UserId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_tbl_UserId] ON [dbo].[RefreshTokens_tbl]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationships_tbl_FollowerId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_Relationships_tbl_FollowerId] ON [dbo].[Relationships_tbl]
(
	[FollowerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationships_tbl_FollowingId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_Relationships_tbl_FollowingId] ON [dbo].[Relationships_tbl]
(
	[FollowingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserPostVote_tbl_PostId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserPostVote_tbl_PostId] ON [dbo].[UserPostVote_tbl]
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserPostVote_tbl_UserId]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserPostVote_tbl_UserId] ON [dbo].[UserPostVote_tbl]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_tbl_Email]    Script Date: 6/17/2023 9:24:33 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_tbl_Email] ON [dbo].[Users_tbl]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Posts_tbl] ADD  CONSTRAINT [DF_Posts_tbl_IsPending]  DEFAULT ((0)) FOR [IsPending]
GO
ALTER TABLE [dbo].[CategoryPost]  WITH CHECK ADD  CONSTRAINT [FK_CategoryPost_Categories_table_CategoriesId] FOREIGN KEY([CategoriesId])
REFERENCES [dbo].[Categories_table] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoryPost] CHECK CONSTRAINT [FK_CategoryPost_Categories_table_CategoriesId]
GO
ALTER TABLE [dbo].[CategoryPost]  WITH CHECK ADD  CONSTRAINT [FK_CategoryPost_Posts_tbl_PostsId] FOREIGN KEY([PostsId])
REFERENCES [dbo].[Posts_tbl] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoryPost] CHECK CONSTRAINT [FK_CategoryPost_Posts_tbl_PostsId]
GO
ALTER TABLE [dbo].[Comments_tbl]  WITH CHECK ADD  CONSTRAINT [FK_Comments_tbl_Posts_tbl_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts_tbl] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments_tbl] CHECK CONSTRAINT [FK_Comments_tbl_Posts_tbl_PostId]
GO
ALTER TABLE [dbo].[Comments_tbl]  WITH CHECK ADD  CONSTRAINT [FK_Comments_tbl_Users_tbl_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users_tbl] ([Id])
GO
ALTER TABLE [dbo].[Comments_tbl] CHECK CONSTRAINT [FK_Comments_tbl_Users_tbl_UserId]
GO
ALTER TABLE [dbo].[Posts_tbl]  WITH CHECK ADD  CONSTRAINT [FK_Posts_tbl_Users_tbl_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users_tbl] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Posts_tbl] CHECK CONSTRAINT [FK_Posts_tbl_Users_tbl_UserId]
GO
ALTER TABLE [dbo].[RefreshTokens_tbl]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_tbl_Users_tbl_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users_tbl] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RefreshTokens_tbl] CHECK CONSTRAINT [FK_RefreshTokens_tbl_Users_tbl_UserId]
GO
ALTER TABLE [dbo].[Relationships_tbl]  WITH CHECK ADD  CONSTRAINT [FK_Relationships_tbl_Users_tbl_FollowerId] FOREIGN KEY([FollowerId])
REFERENCES [dbo].[Users_tbl] ([Id])
GO
ALTER TABLE [dbo].[Relationships_tbl] CHECK CONSTRAINT [FK_Relationships_tbl_Users_tbl_FollowerId]
GO
ALTER TABLE [dbo].[Relationships_tbl]  WITH CHECK ADD  CONSTRAINT [FK_Relationships_tbl_Users_tbl_FollowingId] FOREIGN KEY([FollowingId])
REFERENCES [dbo].[Users_tbl] ([Id])
GO
ALTER TABLE [dbo].[Relationships_tbl] CHECK CONSTRAINT [FK_Relationships_tbl_Users_tbl_FollowingId]
GO
ALTER TABLE [dbo].[UserPostVote_tbl]  WITH CHECK ADD  CONSTRAINT [FK_UserPostVote_tbl_Posts_tbl_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts_tbl] ([Id])
GO
ALTER TABLE [dbo].[UserPostVote_tbl] CHECK CONSTRAINT [FK_UserPostVote_tbl_Posts_tbl_PostId]
GO
ALTER TABLE [dbo].[UserPostVote_tbl]  WITH CHECK ADD  CONSTRAINT [FK_UserPostVote_tbl_Users_tbl_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users_tbl] ([Id])
GO
ALTER TABLE [dbo].[UserPostVote_tbl] CHECK CONSTRAINT [FK_UserPostVote_tbl_Users_tbl_UserId]
GO
USE [master]
GO
ALTER DATABASE [Writing] SET  READ_WRITE 
GO
