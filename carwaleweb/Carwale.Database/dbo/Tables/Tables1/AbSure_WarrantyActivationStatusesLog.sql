CREATE TABLE [dbo].[AbSure_WarrantyActivationStatusesLog] (
    [ID]                                  INT           IDENTITY (1, 1) NOT NULL,
    [AbSure_WarrantyActivationStatusesId] INT           NULL,
    [AbSureCarDetailsId]                  INT           NULL,
    [UserId]                              INT           NULL,
    [EntryDate]                           DATETIME      NULL,
    [RejectionReason]                     VARCHAR (200) NULL,
    [IsActive]                            BIT           NULL,
    [IsCarTradeWarranty]                  BIT           NULL,
    CONSTRAINT [PK_AbSWarrantyActLog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

