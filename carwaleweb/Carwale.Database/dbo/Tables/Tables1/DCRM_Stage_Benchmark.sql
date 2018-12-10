CREATE TABLE [dbo].[DCRM_Stage_Benchmark] (
    [StageBenchmark_Id] INT          IDENTITY (1, 1) NOT NULL,
    [StageId]           TINYINT      NULL,
    [BenchmarkId]       TINYINT      NULL,
    [CityId]            SMALLINT     NULL,
    [City]              VARCHAR (50) NOT NULL,
    [State]             VARCHAR (30) NOT NULL,
    [Zone]              VARCHAR (30) NULL,
    [CityBenchmark]     INT          NULL,
    [StateBenchmark]    INT          NULL,
    [ZoneBenchmark]     INT          NULL,
    CONSTRAINT [FK_DCRM_Stage_Benchmark_DCRM_Benchmarks] FOREIGN KEY ([BenchmarkId]) REFERENCES [dbo].[DCRM_Benchmarks] ([BenchmarkId]),
    CONSTRAINT [FK_DCRM_Stage_Benchmark_DCRM_Stages] FOREIGN KEY ([StageId]) REFERENCES [dbo].[DCRM_Stages] ([StageId])
);

