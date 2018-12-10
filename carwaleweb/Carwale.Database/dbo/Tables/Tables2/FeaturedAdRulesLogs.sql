CREATE TABLE [dbo].[FeaturedAdRulesLogs] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [FeaturedAdRuleId] INT           NOT NULL,
    [FeaturedAdId]     INT           NOT NULL,
    [StateId]          INT           NOT NULL,
    [CityId]           INT           NOT NULL,
    [ZoneId]           INT           NULL,
    [TargetVersion]    INT           NOT NULL,
    [FeaturedVersion]  INT           NOT NULL,
    [UpdatedOn]        DATETIME      NOT NULL,
    [UpdatedBy]        INT           NOT NULL,
    [Remarks]          VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_FeaturedAdRulesLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

