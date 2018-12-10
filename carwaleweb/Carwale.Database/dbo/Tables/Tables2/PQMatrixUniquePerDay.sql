CREATE TABLE [dbo].[PQMatrixUniquePerDay] (
    [Day]           INT          NULL,
    [Month]         INT          NULL,
    [Year]          INT          NULL,
    [CNT]           INT          NULL,
    [CityId]        NUMERIC (18) NULL,
    [ForwardedLead] BIT          NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_PQMatrixUniquePerDay_Month_Year]
    ON [dbo].[PQMatrixUniquePerDay]([Month] ASC, [Year] ASC);

