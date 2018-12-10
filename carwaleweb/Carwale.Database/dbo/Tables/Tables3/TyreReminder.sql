CREATE TABLE [dbo].[TyreReminder] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]       VARCHAR (100) NOT NULL,
    [Email]      VARCHAR (100) NOT NULL,
    [Mobile]     VARCHAR (20)  NOT NULL,
    [TyreAge]    INT           NOT NULL,
    [TyreKms]    NUMERIC (18)  NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_TyreReminder_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TyreReminder] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

