CREATE TABLE [dbo].[DCRM_OutletCountLog] (
    [DealerId]      INT      NOT NULL,
    [DealerType]    TINYINT  NOT NULL,
    [OutletCount]   SMALLINT NOT NULL,
    [CaptureDate]   DATETIME NOT NULL,
    [ApplicationId] TINYINT  NOT NULL
);

