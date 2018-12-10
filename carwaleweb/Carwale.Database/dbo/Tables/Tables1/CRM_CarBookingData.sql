CREATE TABLE [dbo].[CRM_CarBookingData] (
    [Id]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarBasicDataId]     NUMERIC (18)   NOT NULL,
    [BookingStatusId]    INT            NOT NULL,
    [BookingRequestDate] DATETIME       NOT NULL,
    [BookingDate]        DATETIME       NULL,
    [Color]              VARCHAR (100)  NULL,
    [CreatedOn]          DATETIME       NOT NULL,
    [UpdatedOn]          DATETIME       NOT NULL,
    [UpdatedBy]          NUMERIC (18)   NOT NULL,
    [Comments]           VARCHAR (1000) NULL,
    [RegisterPersonName] VARCHAR (200)  NULL,
    [NIFeedback]         BIT            NULL,
    [NoFeedbackContact]  BIT            NULL,
    CONSTRAINT [PK_CRM_CarBookingData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CRM_CarBookingData_CRM_CarBasicData] FOREIGN KEY ([CarBasicDataId]) REFERENCES [dbo].[CRM_CarBasicData] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarBookingData_CarBasicDataId]
    ON [dbo].[CRM_CarBookingData]([CarBasicDataId] ASC);

