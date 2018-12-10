CREATE TABLE [dbo].[CRM_ADM_AgencyLeadVerifier] (
    [UserId]    NUMERIC (18) NOT NULL,
    [Type]      SMALLINT     CONSTRAINT [DF_CRM_ADM_AgencyLeadVerifier_Type] DEFAULT ((1)) NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_CRM_ADM_AgencyLeadVerifier_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy] NUMERIC (18) NULL,
    CONSTRAINT [PK_CRM_ADM_AgencyLeadVerifier] PRIMARY KEY CLUSTERED ([UserId] ASC, [Type] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-OEM Other Leads, 2-Research Other Leads, 3-CarWale Other Leads, 4-Skoda CW Leads, 5-Skoda OEM Leads, 6-Skoda Research Leads, 7-Volks CW Leads, 8-Volks OEM Leads, 9-Volks Research Leads', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_ADM_AgencyLeadVerifier', @level2type = N'COLUMN', @level2name = N'Type';

