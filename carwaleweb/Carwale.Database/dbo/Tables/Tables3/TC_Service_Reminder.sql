CREATE TABLE [dbo].[TC_Service_Reminder] (
    [TC_Service_ReminderId]    INT           IDENTITY (1, 1) NOT NULL,
    [RegistrationNumber]       VARCHAR (50)  NOT NULL,
    [VersionId]                INT           NOT NULL,
    [BranchId]                 INT           NOT NULL,
    [Kms]                      INT           NULL,
    [CustomerId]               INT           NOT NULL,
    [ServiceType]              TINYINT       NULL,
    [ServiceDueDate]           DATETIME      NULL,
    [LastServiceDate]          DATETIME      NULL,
    [EntryDate]                DATETIME      NOT NULL,
    [ServiceCompletedDate]     DATETIME      NULL,
    [ModifiedDate]             DATETIME      NULL,
    [ModifiedBy]               INT           NULL,
    [LeadOwnerId]              INT           NULL,
    [CarDetails]               VARCHAR (250) NULL,
    [Comments]                 VARCHAR (500) NULL,
    [RegistrationNumberSearch] VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_TC_Service_Reminder] PRIMARY KEY CLUSTERED ([TC_Service_ReminderId] ASC)
);

