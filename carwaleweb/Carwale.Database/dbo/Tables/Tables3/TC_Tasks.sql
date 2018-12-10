CREATE TABLE [dbo].[TC_Tasks] (
    [Id]               SMALLINT      IDENTITY (14, 1) NOT NULL,
    [CategoryId]       SMALLINT      NOT NULL,
    [TaskName]         VARCHAR (50)  NOT NULL,
    [TaskDescription]  VARCHAR (200) NULL,
    [EntryDate]        DATETIME      CONSTRAINT [DF_TC_Tasks1_EntryDate] DEFAULT (getdate()) NOT NULL,
    [IsActive]         BIT           CONSTRAINT [DF_TC_Tasks1_IsActive] DEFAULT ((1)) NOT NULL,
    [IsVisible]        BIT           CONSTRAINT [DF_TC_Tasks1_IsVisible] DEFAULT ((1)) NOT NULL,
    [TC_DealerTypeId]  TINYINT       NULL,
    [TC_InquiryTypeId] SMALLINT      NULL,
    [SubCategoryId]    SMALLINT      NULL,
    CONSTRAINT [PK_TC_Tasks1] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 means task will not display to manage roll,this task will define that this user is Super admin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_Tasks', @level2type = N'COLUMN', @level2name = N'IsVisible';

