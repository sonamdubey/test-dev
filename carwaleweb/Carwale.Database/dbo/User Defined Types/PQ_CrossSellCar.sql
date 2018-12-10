CREATE TYPE [dbo].[PQ_CrossSellCar] AS TABLE (
    [Id]               INT NULL,
    [TargetVersion]    INT NULL,
    [CrossSellVersion] INT NULL,
    [StateId]          INT NULL,
    [CityId]           INT NULL,
    [ZoneId]           INT NULL,
    [TemplateId]       INT NULL);

