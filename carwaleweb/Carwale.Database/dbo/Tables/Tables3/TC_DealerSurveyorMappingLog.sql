CREATE TABLE [dbo].[TC_DealerSurveyorMappingLog] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]         BIGINT       NULL,
    [SurveyorMobileNo] VARCHAR (15) NULL,
    [SurveyorId]       BIGINT       NULL,
    [CreatedOn]        DATETIME     NULL,
    [CreatedBy]        BIGINT       NULL,
    [EntryDateLog]     DATETIME     NULL,
    [IsActive]         BIT          NULL,
    [ModifiedBy]       BIGINT       NULL,
    [ModifiedDate]     DATETIME     NULL
);

