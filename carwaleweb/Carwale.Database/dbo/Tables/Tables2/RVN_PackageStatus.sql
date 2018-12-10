CREATE TABLE [dbo].[RVN_PackageStatus] (
    [PackageStatusID] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Status]          VARCHAR (30)  NOT NULL,
    [Description]     VARCHAR (100) NULL,
    [IsActive]        BIT           CONSTRAINT [DF_PackageStatus_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn]       DATETIME      NULL,
    CONSTRAINT [PK_PackageStatus] PRIMARY KEY CLUSTERED ([PackageStatusID] ASC)
);

