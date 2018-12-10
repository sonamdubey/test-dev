CREATE TABLE [dbo].[Microsite_Career] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]        NUMERIC (18)  NOT NULL,
    [Name]            VARCHAR (50)  NOT NULL,
    [EmailId]         VARCHAR (50)  NOT NULL,
    [MobileNumber]    VARCHAR (15)  NOT NULL,
    [PositionApplied] VARCHAR (150) NULL,
    [LinkedinUrl]     VARCHAR (250) NULL,
    [EntryDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_Microsite_Career] PRIMARY KEY CLUSTERED ([Id] ASC)
);

