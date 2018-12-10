CREATE TABLE [dbo].[CRM_CarCustomerServiceRequests] (
    [Id]                  NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarBasicDataId]      NUMERIC (18)  NOT NULL,
    [TDRequest]           BIT           NOT NULL,
    [BookingRequest]      BIT           NOT NULL,
    [TDDate]              DATETIME      NULL,
    [TDLocation]          VARCHAR (500) NULL,
    [InterestedInFinance] BIT           NULL,
    CONSTRAINT [PK_CRM_CustomerServiceRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CRM_CarCustomerServiceRequests_CRM_CarBasicData] FOREIGN KEY ([CarBasicDataId]) REFERENCES [dbo].[CRM_CarBasicData] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarCustomerServiceRequests_CarBasicDataId]
    ON [dbo].[CRM_CarCustomerServiceRequests]([CarBasicDataId] ASC)
    INCLUDE([TDRequest], [TDDate], [TDLocation]);

