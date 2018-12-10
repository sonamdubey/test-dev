CREATE TABLE [dbo].[BhartiAxa_InsuranceDiscounts] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [AgentNumber]     NVARCHAR (255) NULL,
    [MakeID]          NVARCHAR (255) NULL,
    [ModelID]         CHAR (50)      NULL,
    [State]           NVARCHAR (255) NULL,
    [City]            CHAR (50)      NULL,
    [Disount]         FLOAT (53)     NULL,
    [RenewalDiscount] NVARCHAR (255) NULL,
    [BlackListed]     NVARCHAR (255) NULL,
    [ProductType]     CHAR (10)      NULL,
    [CityId]          INT            NULL,
    CONSTRAINT [PK_BhartiAxa_InsuranceDiscounts] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_BhartiAxa_InsuranceDiscounts_ModelId]
    ON [dbo].[BhartiAxa_InsuranceDiscounts]([ModelID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BhartiAxa_InsuranceDiscounts_City]
    ON [dbo].[BhartiAxa_InsuranceDiscounts]([City] ASC);

