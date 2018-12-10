CREATE TYPE [dbo].[TC_TMPageWisePercentageChange] AS TABLE (
    [Id]               INT        NULL,
    [FieldId]          INT        NULL,
    [CarId]            INT        NULL,
    [PercentageChange] FLOAT (53) NULL,
    [NewValue]         INT        NULL);

