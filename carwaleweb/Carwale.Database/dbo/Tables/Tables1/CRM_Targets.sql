CREATE TABLE [dbo].[CRM_Targets] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Type]            NUMERIC (18) NULL,
    [Value]           NUMERIC (18) NULL,
    [Date]            DATETIME     NULL,
    [Brand]           VARCHAR (50) NULL,
    [TargetPeriod]    NUMERIC (18) NULL,
    [LastUpdatedDate] DATETIME     NULL,
    CONSTRAINT [PK_CRM_Targets] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Targets]
    ON [dbo].[CRM_Targets]([Brand] ASC, [Date] ASC, [Type] ASC);

