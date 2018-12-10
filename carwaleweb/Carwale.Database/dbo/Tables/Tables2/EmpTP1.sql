CREATE TABLE [dbo].[EmpTP1] (
    [SNo]         INT            IDENTITY (117, 1) NOT NULL,
    [Name]        NVARCHAR (255) NULL,
    [Code]        NVARCHAR (255) NULL,
    [Department]  NVARCHAR (255) NULL,
    [Designation] NVARCHAR (255) NULL,
    [JoinedOn]    DATETIME       NULL,
    [LeftOn]      NVARCHAR (255) NULL,
    [Leftondate]  DATETIME       NULL,
    [oprid]       INT            NULL,
    CONSTRAINT [PK_EmpTP] PRIMARY KEY CLUSTERED ([SNo] ASC)
);

