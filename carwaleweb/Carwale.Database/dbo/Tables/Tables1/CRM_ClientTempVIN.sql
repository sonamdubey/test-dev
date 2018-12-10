CREATE TABLE [dbo].[CRM_ClientTempVIN] (
    [PendingId]     NUMERIC (18)  NOT NULL,
    [EngNo]         VARCHAR (50)  NULL,
    [ChassisNo]     VARCHAR (50)  NULL,
    [RegNo]         VARCHAR (50)  NULL,
    [RegPersonName] VARCHAR (100) NULL,
    CONSTRAINT [PK_CRM_ClientTempVIN] PRIMARY KEY CLUSTERED ([PendingId] ASC) WITH (FILLFACTOR = 90)
);

