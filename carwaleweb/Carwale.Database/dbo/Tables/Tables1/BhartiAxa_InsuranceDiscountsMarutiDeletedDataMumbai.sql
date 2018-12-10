CREATE TABLE [dbo].[BhartiAxa_InsuranceDiscountsMarutiDeletedDataMumbai] (
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
    [CityId]          INT            NULL
);

