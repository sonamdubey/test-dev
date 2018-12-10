CREATE TABLE [dbo].[CRM_Customers] (
    [ID]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FirstName]          VARCHAR (200)  NOT NULL,
    [LastName]           VARCHAR (100)  NOT NULL,
    [Email]              VARCHAR (200)  NOT NULL,
    [Landline]           VARCHAR (50)   NOT NULL,
    [Mobile]             VARCHAR (50)   NOT NULL,
    [CityId]             NUMERIC (18)   NOT NULL,
    [AreaId]             BIGINT         NULL,
    [Source]             VARCHAR (200)  NOT NULL,
    [CWCustId]           NUMERIC (18)   NOT NULL,
    [IsVerified]         BIT            CONSTRAINT [DF_CRM_Customers_IsVerified] DEFAULT ((0)) NOT NULL,
    [IsFake]             BIT            CONSTRAINT [DF_CRM_Customers_IsFake] DEFAULT ((0)) NOT NULL,
    [CreatedOn]          DATETIME       NOT NULL,
    [UpdatedOn]          DATETIME       NOT NULL,
    [Comments]           VARCHAR (1500) NULL,
    [AlternateEmail]     VARCHAR (200)  NULL,
    [AlternateContactNo] VARCHAR (100)  NULL,
    [UpdatedBy]          NUMERIC (18)   CONSTRAINT [DF_CRM_Customers_UpdatedBy] DEFAULT ((-1)) NOT NULL,
    [IsActive]           BIT            CONSTRAINT [DF_CRM_Customers_IsActive] DEFAULT ((0)) NOT NULL,
    [ActiveLeadId]       NUMERIC (18)   CONSTRAINT [DF_CRM_Customers_ActiveLeadId] DEFAULT ((-1)) NOT NULL,
    [ActiveLeadDate]     DATETIME       NULL,
    [ActiveLeadGroupId]  INT            NULL,
    [Salutation]         VARCHAR (10)   NULL,
    [BlockDate]          DATETIME       NULL,
    [BlockReason]        INT            NULL,
    CONSTRAINT [PK_CNS_Customers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Customers_Mobile]
    ON [dbo].[CRM_Customers]([Mobile] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_Customers_IsActive_IsVerified_IsFake_ActiveLeadId]
    ON [dbo].[CRM_Customers]([IsVerified] ASC, [IsFake] ASC, [IsActive] ASC, [ActiveLeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Customers_CityId]
    ON [dbo].[CRM_Customers]([CityId] ASC)
    INCLUDE([ID]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_Customers__CWCustId]
    ON [dbo].[CRM_Customers]([CWCustId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_Customers_Email]
    ON [dbo].[CRM_Customers]([Email] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Customers_ActiveLeadId]
    ON [dbo].[CRM_Customers]([ActiveLeadId] ASC);


GO
CREATE TRIGGER [dbo].[TRG_CRM_Customers]
ON dbo.CRM_Customers
AFTER INSERT , UPDATE 
AS
IF UPDATE (CityId) 
BEGIN
declare @CustomerId bigint

SELECT @CustomerId=CC.Id
FROM CRM_Customers as CC
JOIN inserted as i on CC.id=i.id
  
EXEC CRM.LSUpdateLeadScore 7, -1, @CustomerId
END

GO
DISABLE TRIGGER [dbo].[TRG_CRM_Customers]
    ON [dbo].[CRM_Customers];

