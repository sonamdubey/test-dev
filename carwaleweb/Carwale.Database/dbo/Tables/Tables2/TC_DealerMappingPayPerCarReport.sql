CREATE TABLE [dbo].[TC_DealerMappingPayPerCarReport] (
    [DealerId]                       INT           NULL,
    [Organization]                   VARCHAR (150) NULL,
    [TC_OrganizationListPayPerCarId] INT           NULL,
    [IsActive]                       BIT           CONSTRAINT [TC_DealerMappingPayPerCarReport_IsActive] DEFAULT ((1)) NULL,
    [DealerRemovedFromReportOn]      DATETIME      NULL,
    [MappingCreatedOn]               DATETIME      DEFAULT (getdate()) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DealerMappingPayPerCarReport_DealerId]
    ON [dbo].[TC_DealerMappingPayPerCarReport]([DealerId] ASC);

