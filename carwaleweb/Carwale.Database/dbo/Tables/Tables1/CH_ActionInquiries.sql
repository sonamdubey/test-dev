CREATE TABLE [dbo].[CH_ActionInquiries] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [InquiryId]    INT      NULL,
    [Status]       TINYINT  NULL,
    [ApprovedBy]   INT      NULL,
    [ApprovedDate] DATETIME NULL,
    CONSTRAINT [PK_CH_ActionInquiries] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Status =1 for takes live and 9 for Remove ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CH_ActionInquiries', @level2type = N'COLUMN', @level2name = N'Status';

