CREATE TABLE [dbo].[tempTC_CustomerDetails] (
    [id]            INT           NOT NULL,
    [CustomerName]  VARCHAR (100) NOT NULL,
    [Email]         VARCHAR (100) NULL,
    [Mobile]        VARCHAR (15)  NULL,
    [IsActive]      BIT           NOT NULL,
    [Address]       VARCHAR (200) NULL,
    [City]          INT           NULL,
    [Pincode]       VARCHAR (50)  NULL,
    [Dob]           DATE          NULL,
    [Anniversary]   DATE          NULL,
    [BranchId]      NUMERIC (18)  NULL,
    [ModifiedDate]  DATETIME      NULL,
    [EntryDate]     DATETIME      NULL,
    [ModifiedBy]    INT           NULL,
    [Buytime]       VARCHAR (20)  NULL,
    [Location]      VARCHAR (50)  NULL,
    [Comments]      VARCHAR (400) NULL,
    [CreatedBy]     BIGINT        NULL,
    [CW_CustomerId] BIGINT        NULL,
    [ToBeDeleted]   BIT           NULL
);

