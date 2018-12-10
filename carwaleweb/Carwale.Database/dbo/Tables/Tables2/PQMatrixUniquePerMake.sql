CREATE TABLE [dbo].[PQMatrixUniquePerMake] (
    [PQMatrix_Id]   BIGINT   IDENTITY (1, 1) NOT NULL,
    [MakeId]        SMALLINT NOT NULL,
    [PQCNT]         BIGINT   NULL,
    [Day]           TINYINT  NULL,
    [Month]         TINYINT  NULL,
    [Year]          SMALLINT NULL,
    [ForwardedLead] BIT      NOT NULL,
    [CityId]        INT      NULL,
    [UpdatedOn]     DATETIME NOT NULL
);

