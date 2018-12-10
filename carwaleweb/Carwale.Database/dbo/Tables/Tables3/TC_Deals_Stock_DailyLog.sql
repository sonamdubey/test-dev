CREATE TABLE [dbo].[TC_Deals_Stock_DailyLog] (
    [AsOnDate]       DATE          NULL,
    [Id]             INT           NULL,
    [BranchId]       INT           NULL,
    [CarVersionId]   INT           NULL,
    [MakeYear]       DATE          NULL,
    [VersionColorId] INT           NULL,
    [InteriorColor]  VARCHAR (50)  NULL,
    [EnteredOn]      DATETIME      NULL,
    [EnteredBy]      INT           NULL,
    [LastUpdatedOn]  DATETIME      NULL,
    [LastUpdatedBy]  INT           NULL,
    [Offers]         VARCHAR (500) NULL,
    [IsApproved]     BIT           NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Deals_Stock_DailyLog_AsOnDate]
    ON [dbo].[TC_Deals_Stock_DailyLog]([AsOnDate] ASC);

