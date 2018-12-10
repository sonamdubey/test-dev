CREATE TABLE [dbo].[CRM_TATAPincodes] (
    [DealerId] NUMERIC (18) NOT NULL,
    [Pincode]  INT          NOT NULL,
    CONSTRAINT [PK_CRM_TATAPincodes] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

