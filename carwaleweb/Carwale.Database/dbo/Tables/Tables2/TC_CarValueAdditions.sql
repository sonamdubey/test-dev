CREATE TABLE [dbo].[TC_CarValueAdditions] (
    [TC_CarValueAdditionsId] SMALLINT      IDENTITY (1, 1) NOT NULL,
    [ValueAddName]           VARCHAR (50)  NOT NULL,
    [Logo]                   VARCHAR (100) NULL,
    [Description]            VARCHAR (100) NULL,
    [IsActive]               BIT           CONSTRAINT [DF_TC_CarValueAdditions_IsActive] DEFAULT ((1)) NOT NULL
);

