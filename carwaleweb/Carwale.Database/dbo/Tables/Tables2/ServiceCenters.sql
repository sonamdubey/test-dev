CREATE TABLE [dbo].[ServiceCenters] (
    [DealerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ServiceCenters] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

