CREATE TABLE [dbo].[NCD_LeadStatus] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [StatusName]     VARCHAR (80) NULL,
    [IsActive]       BIT          NULL,
    [LeadStatusType] BIT          NULL,
    [IsClosed]       BIT          CONSTRAINT [DF_NCD_LeadStatus_IsClosed] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_NCD_LeadStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Accepted,0=Rejected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_LeadStatus', @level2type = N'COLUMN', @level2name = N'LeadStatusType';

