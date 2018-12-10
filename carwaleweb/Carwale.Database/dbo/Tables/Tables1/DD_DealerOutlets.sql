CREATE TABLE [dbo].[DD_DealerOutlets] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [OutletName]       VARCHAR (250) NOT NULL,
    [DD_DealerNamesId] INT           NOT NULL,
    [OutletType]       TINYINT       NOT NULL,
    [MakeId]           INT           NULL,
    [Website]          VARCHAR (100) NULL,
    [EMailId]          VARCHAR (100) NULL,
    [DayType]          INT           NULL,
    [ContactHours]     VARCHAR (20)  NULL,
    [Day]              VARCHAR (15)  NULL,
    [CreatedBy]        INT           NOT NULL,
    [CreatedOn]        DATETIME      NOT NULL,
    CONSTRAINT [PK_DD_DealerOutlets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

