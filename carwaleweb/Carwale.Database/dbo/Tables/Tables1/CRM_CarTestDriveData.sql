CREATE TABLE [dbo].[CRM_CarTestDriveData] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarBasicDataId]  NUMERIC (18)   NOT NULL,
    [TDRequestDate]   DATETIME       NOT NULL,
    [TDCompletedDate] DATETIME       NULL,
    [TDLocationType]  SMALLINT       NOT NULL,
    [TDStatusId]      SMALLINT       NOT NULL,
    [DealerId]        NUMERIC (18)   NOT NULL,
    [ContactPerson]   VARCHAR (50)   NULL,
    [Contact]         VARCHAR (50)   NULL,
    [Comments]        VARCHAR (1000) NULL,
    [CreatedOn]       DATETIME       NOT NULL,
    [UpdatedOn]       DATETIME       NOT NULL,
    [UpdatedBy]       NUMERIC (18)   NOT NULL,
    CONSTRAINT [PK_CRM_CarTestDriveData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarTestDriveData_CarBasicDataId]
    ON [dbo].[CRM_CarTestDriveData]([CarBasicDataId] ASC);

