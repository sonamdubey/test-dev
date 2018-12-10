CREATE TABLE [dbo].[DCRM_MFCMappedCities] (
    [ID]               INT      IDENTITY (1, 1) NOT NULL,
    [CityID]           INT      NOT NULL,
    [IsActive]         BIT      NOT NULL,
    [UpdatedBy]        INT      NOT NULL,
    [UpdatedOn]        DATETIME NOT NULL,
    [ProcurementLeads] INT      NULL,
    [LeadsSent]        INT      NULL,
    CONSTRAINT [PK_DCRM_MFCMappedCities] PRIMARY KEY CLUSTERED ([ID] ASC)
);

