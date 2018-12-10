CREATE TABLE [dbo].[AbSure_CarSurveyorMapping] (
    [AbSure_CarSurveyorMappingId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [BranchId]                    INT          NULL,
    [TC_UserId]                   INT          NULL,
    [AbSure_CarDetailsId]         INT          NULL,
    [EntryDate]                   DATETIME     CONSTRAINT [DF_AbSure_CarSurveyorMapping_EntryDate] DEFAULT (getdate()) NULL,
    [TC_StockId]                  INT          NULL,
    [UpdatedBy]                   INT          NULL,
    [UpdatedOn]                   DATETIME     NULL,
    CONSTRAINT [PK_AbSure_CarSurveyorMapping] PRIMARY KEY CLUSTERED ([AbSure_CarSurveyorMappingId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AbSure_CarSurveyorMapping_TC_UserId]
    ON [dbo].[AbSure_CarSurveyorMapping]([TC_UserId] ASC)
    INCLUDE([AbSure_CarDetailsId]);


GO
CREATE NONCLUSTERED INDEX [Ix_AbSure_CarSurveyorMapping_AbSure_CarDetailsId]
    ON [dbo].[AbSure_CarSurveyorMapping]([AbSure_CarDetailsId] ASC);

