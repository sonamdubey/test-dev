CREATE TABLE [dbo].[DCRM_Benchmarks] (
    [BenchmarkId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [Descr]       VARCHAR (50) NULL,
    CONSTRAINT [PK_DCRM_Benchmarks] PRIMARY KEY CLUSTERED ([BenchmarkId] ASC)
);

