CREATE TABLE [dbo].[DCRM_CarsORData] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [InquiryId] NUMERIC (18) NOT NULL,
    [ORKm]      BIT          NULL,
    [ORPrice]   BIT          NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_DCRM_CarsORData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn] DATETIME     NULL,
    [UpdatedBy] NUMERIC (18) NULL,
    CONSTRAINT [PK_DCRM_CarORData] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarsORData_ORKm]
    ON [dbo].[DCRM_CarsORData]([ORKm] ASC)
    INCLUDE([InquiryId]);


GO
CREATE NONCLUSTERED INDEX [IX_DCRM_CarsORData_InquiryId]
    ON [dbo].[DCRM_CarsORData]([InquiryId] ASC);

