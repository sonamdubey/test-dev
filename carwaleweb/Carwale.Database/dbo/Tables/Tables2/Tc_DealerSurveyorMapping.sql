CREATE TABLE [dbo].[Tc_DealerSurveyorMapping] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]         BIGINT       NULL,
    [SurveyorMobileNo] VARCHAR (15) NULL,
    [SurveyorId]       BIGINT       NULL,
    [CreatedBy]        BIGINT       NULL,
    [EntryDate]        DATETIME     CONSTRAINT [DF_Tc_DealerSurveyorMapping_EntryDate] DEFAULT (getdate()) NULL,
    [IsActive]         BIT          CONSTRAINT [DF_Tc_DealerSurveyorMapping_IsActive] DEFAULT ((1)) NULL,
    [ModifiedBy]       BIGINT       NULL,
    [ModifiedDate]     DATETIME     NULL
);

