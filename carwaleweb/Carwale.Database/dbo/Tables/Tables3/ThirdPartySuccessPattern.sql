CREATE TABLE [dbo].[ThirdPartySuccessPattern] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [LeadSettingId] INT           NOT NULL,
    [Pattern]       VARCHAR (200) NOT NULL,
    [IsActive]      BIT           NOT NULL,
    [LastUpdatedOn] DATETIME      NOT NULL,
    [LastUpdatedBy] INT           NOT NULL,
    CONSTRAINT [PK_ThirdPartySuccessPattern] PRIMARY KEY CLUSTERED ([Id] ASC)
);

