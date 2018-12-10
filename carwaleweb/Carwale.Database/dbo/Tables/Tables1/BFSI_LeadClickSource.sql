CREATE TABLE [dbo].[BFSI_LeadClickSource] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [ClickSource] VARCHAR (50) NOT NULL,
    [UtmCode]     VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Finance_LeadClickSource] PRIMARY KEY CLUSTERED ([Id] ASC)
);

