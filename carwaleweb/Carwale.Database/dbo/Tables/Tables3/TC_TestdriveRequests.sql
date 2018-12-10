CREATE TABLE [dbo].[TC_TestdriveRequests] (
    [TC_TestDriveId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesId] BIGINT       NULL,
    [ModelId]        SMALLINT     NOT NULL,
    [FuelType]       TINYINT      NULL,
    [Transmission]   TINYINT      NULL,
    CONSTRAINT [PK_TC_TestdriveRequests] PRIMARY KEY CLUSTERED ([TC_TestDriveId] ASC)
);

