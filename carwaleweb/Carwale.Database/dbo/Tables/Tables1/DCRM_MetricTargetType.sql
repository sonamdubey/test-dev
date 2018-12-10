CREATE TABLE [dbo].[DCRM_MetricTargetType] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NULL,
    [Description] VARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

