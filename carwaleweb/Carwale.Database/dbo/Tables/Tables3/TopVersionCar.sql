CREATE TABLE [dbo].[TopVersionCar] (
    [Modelid]     SMALLINT NULL,
    [VersionId]   INT      NULL,
    [PQCount]     BIGINT   NULL,
    [UpdatedDate] DATETIME DEFAULT (getdate()) NULL
);

