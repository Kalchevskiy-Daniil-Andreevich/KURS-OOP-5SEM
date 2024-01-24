USE [master]
GO

/****** Object:  Database [cptoop]    Script Date: 21.12.2023 16:49:10 ******/
CREATE DATABASE [publ]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'publ', FILENAME = N'D:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\publ.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'publ_log', FILENAME = N'D:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\publ_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [publ].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [publ] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [publ] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [publ] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [publ] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [publ] SET ARITHABORT OFF 
GO

ALTER DATABASE [publ] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [publ] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [publ] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [publ] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [publ] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [publ] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [publ] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [publ] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [publ] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [publ] SET  DISABLE_BROKER 
GO

ALTER DATABASE [publ] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [publ] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [publ] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [publ] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [publ] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [publ] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [publ] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [publ] SET RECOVERY FULL 
GO

ALTER DATABASE [publ] SET  MULTI_USER 
GO

ALTER DATABASE [publ] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [publ] SET DB_CHAINING OFF 
GO

ALTER DATABASE [publ] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [publ] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [publ] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [publ] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [publ] SET QUERY_STORE = ON
GO

ALTER DATABASE [publ] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [publ] SET  READ_WRITE 
GO


