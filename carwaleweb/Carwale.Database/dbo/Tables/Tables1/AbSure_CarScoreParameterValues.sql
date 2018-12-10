CREATE TABLE [dbo].[AbSure_CarScoreParameterValues] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [ParameterId]      INT          NOT NULL,
    [MaxWeightage]     INT          NULL,
    [MinValue]         INT          NULL,
    [MaxValue]         INT          NULL,
    [ConstantValue]    VARCHAR (50) NULL,
    [WeightagePercent] INT          NULL
);

