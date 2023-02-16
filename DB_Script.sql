CREATE TABLE [dbo].[Student] (
    [UniqueID]   INT           IDENTITY (1, 1) NOT NULL,
    [StudentNum] NVARCHAR (50) NULL,
    [Name]       NVARCHAR (50) NULL,
    [Birthday]   NVARCHAR (50) NULL,
    [Email]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED ([UniqueID] ASC)
);

CREATE TABLE [dbo].[Class] (
    [UniqueID] INT           IDENTITY (1, 1) NOT NULL,
    [ClassNum] NVARCHAR (50) NULL,
    [Name]     NVARCHAR (50) NULL,
    [Credit]   INT           NULL,
    [Place]    NVARCHAR (50) NULL,
    [Teacher]  NVARCHAR (50) NULL,
    CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED ([UniqueID] ASC)
);

CREATE TABLE [dbo].[SelectClass] (
    [UniqueID]   INT           IDENTITY (1, 1) NOT NULL,
    [StudentNum] NVARCHAR (50) NULL,
    [ClassNum]   NVARCHAR (50) NULL,
    CONSTRAINT [PK_SelectClass] PRIMARY KEY CLUSTERED ([UniqueID] ASC)
);

