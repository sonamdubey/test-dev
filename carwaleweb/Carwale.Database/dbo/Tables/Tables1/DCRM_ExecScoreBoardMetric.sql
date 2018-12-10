CREATE TABLE [dbo].[DCRM_ExecScoreBoardMetric] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [MetricName]     VARCHAR (200) NULL,
    [BusinessUnitId] INT           NOT NULL,
    [InquiryPointId] INT           NOT NULL,
    [IsActive]       BIT           NOT NULL,
    [AddedBy]        INT           NOT NULL,
    [AddedOn]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      INT           NULL,
    [UpdatedOn]      DATETIME      NULL,
    [TargetType]     TINYINT       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

