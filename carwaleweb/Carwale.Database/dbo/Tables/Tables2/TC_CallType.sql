CREATE TABLE [dbo].[TC_CallType] (
    [TC_CallTypeId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [name]          VARCHAR (50) NULL,
    [IsActive]      BIT          CONSTRAINT [DF_TC_CallType_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_CallTypeId] PRIMARY KEY NONCLUSTERED ([TC_CallTypeId] ASC)
);

