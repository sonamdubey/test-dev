CREATE TABLE [dbo].[LeadSource] (
    [Id]                INT          IDENTITY (1, 1) NOT NULL,
    [PlatformId]        INT          NOT NULL,
    [AdType]            INT          NOT NULL,
    [SourceType]        VARCHAR (50) NULL,
    [LeadClickSourceId] INT          NOT NULL,
    [InquirySourceId]   INT          NOT NULL,
    CONSTRAINT [PK_LeadSource] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 = PQ
2 = Paid cross sell PQ
3 = House cross sell PQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LeadSource', @level2type = N'COLUMN', @level2name = N'AdType';

