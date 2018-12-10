CREATE TABLE [dbo].[OprCurrentDealerStatus] (
    [ID]       NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsClosed] BIT          CONSTRAINT [DF_OprCurrentDealerStatus_IsClosed] DEFAULT (0) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_OprCurrentDealerStatus_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_OprCurrentDealerStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

