CREATE TABLE [dbo].[TC_CustomerDetails_11092011] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]   NUMERIC (18)  NOT NULL,
    [CustomerName] VARCHAR (100) NOT NULL,
    [Email]        VARCHAR (100) NULL,
    [Mobile]       NUMERIC (18)  NULL,
    [IsActive]     BIT           NOT NULL,
    [Address]      VARCHAR (200) NULL,
    [City]         INT           NULL,
    [Pincode]      VARCHAR (50)  NULL,
    [Dob]          DATE          NULL,
    [Anniversary]  DATE          NULL,
    [BranchId]     NUMERIC (18)  NULL,
    [ToBeDeleted]  BIT           CONSTRAINT [DF__TC_Custom__ToBeD__07279A13] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TC_CustomerDetails_11092011] PRIMARY KEY CLUSTERED ([ID] ASC)
);

